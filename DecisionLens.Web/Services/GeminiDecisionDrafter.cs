using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Options;

namespace DecisionLens.Web.Services;

public class GeminiDecisionDrafter : IDecisionDrafter
{
    private readonly HttpClient _http;
    private readonly AiOptions _ai;
    private readonly LimitsOptions _limits;
    private readonly ILogger<GeminiDecisionDrafter> _logger;

    public GeminiDecisionDrafter(
        HttpClient http,
        IOptions<AiOptions> ai,
        IOptions<LimitsOptions> limits,
        ILogger<GeminiDecisionDrafter> logger)
    {
        _http = http;
        _ai = ai.Value;
        _limits = limits.Value;
        _logger = logger;
        _http.Timeout = TimeSpan.FromSeconds(Math.Max(5, _ai.TimeoutSeconds));
    }

    public async Task<DraftOutcome> DraftAsync(string discussion, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(discussion))
        {
            return DraftOutcome.Fail(DraftErrorCode.EmptyInput);
        }
        if (discussion.Length > _limits.MaxInputChars)
        {
            return DraftOutcome.Fail(DraftErrorCode.InputTooLong, _limits.MaxInputChars);
        }
        if (string.IsNullOrWhiteSpace(_ai.ApiKey) || string.IsNullOrWhiteSpace(_ai.Model))
        {
            _logger.LogWarning("AI call skipped: {Code}", DraftErrorCode.NotConfigured);
            return DraftOutcome.Fail(DraftErrorCode.NotConfigured);
        }

        DraftOutcome? last = null;
        for (var attempt = 1; attempt <= 2; attempt++)
        {
            var (outcome, retry) = await TryOnceAsync(discussion, cancellationToken);
            if (outcome.Success)
            {
                return outcome;
            }
            last = outcome;
            if (!retry)
            {
                break;
            }
        }
        return last!;
    }

    private async Task<(DraftOutcome Outcome, bool Retry)> TryOnceAsync(string discussion, CancellationToken ct)
    {
        var url = $"https://generativelanguage.googleapis.com/v1beta/models/{Uri.EscapeDataString(_ai.Model)}:generateContent";
        using var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new StringContent(BuildRequestBody(discussion).ToJsonString(), Encoding.UTF8, "application/json")
        };
        request.Headers.Add("x-goog-api-key", _ai.ApiKey);

        try
        {
            using var response = await _http.SendAsync(request, ct);
            var status = (int)response.StatusCode;

            if (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                _logger.LogWarning("AI call failed: {Code}, HTTP {Status}", DraftErrorCode.ProviderRateLimit, status);
                return (DraftOutcome.Fail(DraftErrorCode.ProviderRateLimit), false);
            }

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("AI call failed: {Code}, HTTP {Status}", DraftErrorCode.ProviderError, status);
                var errorBody = await response.Content.ReadAsStringAsync(ct);
                _logger.LogDebug("Provider error body: {Body}", errorBody.Length > 300 ? errorBody.Substring(0, 300) : errorBody);
                return (DraftOutcome.Fail(DraftErrorCode.ProviderError), status >= 500);
            }

            var text = await ExtractTextAsync(response, ct);
            var outcome = DraftValidator.Validate(text);
            if (!outcome.Success)
            {
                _logger.LogWarning("AI call failed: {Code}, HTTP {Status}", DraftErrorCode.InvalidReply, status);
            }
            return (outcome, true);
        }
        catch (TaskCanceledException) when (!ct.IsCancellationRequested)
        {
            _logger.LogWarning("AI call failed: {Code}", DraftErrorCode.ProviderTimeout);
            return (DraftOutcome.Fail(DraftErrorCode.ProviderTimeout), true);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning("AI call failed: {Code}, {ErrorType}", DraftErrorCode.ProviderError, ex.GetType().Name);
            return (DraftOutcome.Fail(DraftErrorCode.ProviderError), true);
        }
    }

    private static JsonObject BuildRequestBody(string discussion)
    {
        return new JsonObject
        {
            ["systemInstruction"] = new JsonObject
            {
                ["parts"] = new JsonArray(new JsonObject { ["text"] = PromptText.SystemPrompt })
            },
            ["contents"] = new JsonArray(new JsonObject
            {
                ["role"] = "user",
                ["parts"] = new JsonArray(new JsonObject { ["text"] = PromptText.BuildUserMessage(discussion) })
            }),
            ["generationConfig"] = new JsonObject
            {
                ["temperature"] = 0.2,
                ["maxOutputTokens"] = 4096,
                ["responseMimeType"] = "application/json",
                ["responseSchema"] = JsonNode.Parse(PromptText.ResponseSchemaJson)
            }
        };
    }

    private static async Task<string?> ExtractTextAsync(HttpResponseMessage response, CancellationToken ct)
    {
        try
        {
            var json = await response.Content.ReadAsStringAsync(ct);
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (root.TryGetProperty("promptFeedback", out var feedback) && feedback.TryGetProperty("blockReason", out _))
            {
                return null;
            }
            if (!root.TryGetProperty("candidates", out var candidates)
                || candidates.ValueKind != JsonValueKind.Array
                || candidates.GetArrayLength() == 0)
            {
                return null;
            }

            var candidate = candidates[0];
            if (candidate.TryGetProperty("finishReason", out var finish) && finish.GetString() == "MAX_TOKENS")
            {
                return null;
            }

            if (candidate.TryGetProperty("content", out var content)
                && content.TryGetProperty("parts", out var parts)
                && parts.ValueKind == JsonValueKind.Array)
            {
                var sb = new StringBuilder();
                foreach (var part in parts.EnumerateArray())
                {
                    var isThought = part.TryGetProperty("thought", out var thought) && thought.ValueKind == JsonValueKind.True;
                    if (!isThought && part.TryGetProperty("text", out var text) && text.ValueKind == JsonValueKind.String)
                    {
                        sb.Append(text.GetString());
                    }
                }
                return sb.Length == 0 ? null : sb.ToString();
            }
            return null;
        }
        catch (JsonException)
        {
            return null;
        }
    }
}