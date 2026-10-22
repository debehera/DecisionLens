using System.Text.Json;
using DecisionLens.Web.Models;

namespace DecisionLens.Web.Services;

public static class DraftValidator
{
    private static readonly string[] RequiredProperties =
    {
        "title", "decision", "context", "options_considered", "rationale",
        "consequences", "adm_phase", "domain", "tag_confidence", "tag_reason"
    };

    public static DraftOutcome Validate(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return DraftOutcome.Fail(DraftErrorCode.InvalidReply);
        }

        try
        {
            using var doc = JsonDocument.Parse(StripFences(text));
            var root = doc.RootElement;
            if (root.ValueKind != JsonValueKind.Object)
            {
                return DraftOutcome.Fail(DraftErrorCode.InvalidReply);
            }

            var values = new Dictionary<string, string>();
            foreach (var name in RequiredProperties)
            {
                if (!root.TryGetProperty(name, out var element) || element.ValueKind != JsonValueKind.String)
                {
                    return DraftOutcome.Fail(DraftErrorCode.InvalidReply);
                }
                values[name] = element.GetString() ?? "";
            }

            var phase = TogafCatalog.MatchPhase(values["adm_phase"]);
            var domain = TogafCatalog.MatchDomain(values["domain"]);
            if (phase == null || domain == null)
            {
                return DraftOutcome.Fail(DraftErrorCode.InvalidReply);
            }

            var confidence = values["tag_confidence"].Trim().ToLowerInvariant();
            if (confidence != "low" && confidence != "medium" && confidence != "high")
            {
                confidence = "low";
            }

            var record = new DraftedRecord(
                Title: Clean(values["title"], 150),
                Decision: Clean(values["decision"], 4000),
                Context: Clean(values["context"], 4000),
                OptionsConsidered: Clean(values["options_considered"], 4000),
                Rationale: Clean(values["rationale"], 4000),
                Consequences: Clean(values["consequences"], 4000),
                AdmPhase: phase,
                Domain: domain);

            var note = new DraftNote(confidence, Clean(values["tag_reason"], 1000));
            return DraftOutcome.Ok(record, note);
        }
        catch (JsonException)
        {
            return DraftOutcome.Fail(DraftErrorCode.InvalidReply);
        }
    }

    private static string StripFences(string text)
    {
        var t = text.Trim();
        if (t.StartsWith("```"))
        {
            var firstNewline = t.IndexOf('\n');
            if (firstNewline >= 0)
            {
                t = t.Substring(firstNewline + 1);
            }
            if (t.EndsWith("```"))
            {
                t = t.Substring(0, t.Length - 3);
            }
            t = t.Trim();
        }
        return t;
    }

    private static string Clean(string value, int maxLength)
    {
        var v = value.Trim();
        if (v.Length == 0)
        {
            return TogafCatalog.NotStated;
        }
        return v.Length > maxLength ? v.Substring(0, maxLength).TrimEnd() : v;
    }
}