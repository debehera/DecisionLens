using System.Text.Json;
using DecisionLens.Web.Data;
using DecisionLens.Web.Models;
using DecisionLens.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace DecisionLens.Web.Pages;

public record DraftRequest(string? Text);

public class CaptureModel : PageModel
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly IDecisionDrafter _drafter;
    private readonly AppDbContext _db;
    private readonly UsageLimiter _limiter;
    private readonly LimitsOptions _limits;

    public CaptureModel(IDecisionDrafter drafter, AppDbContext db, UsageLimiter limiter, IOptions<LimitsOptions> limits)
    {
        _drafter = drafter;
        _db = db;
        _limiter = limiter;
        _limits = limits.Value;
    }

    [BindProperty]
    public RecordInput Input { get; set; } = new();

    public bool ShowReview { get; private set; }

    public int MaxInputChars => _limits.MaxInputChars;

    public int DraftsLeftToday { get; private set; }

    // JSON for the sample dropdown, read by capture.js
    public string SamplesJson { get; private set; } = "[]";

    public async Task OnGetAsync()
    {
        LoadSamples();
        DraftsLeftToday = await _limiter.GetRemainingAsync();
    }

    // Called by capture.js with fetch: POST /Capture?handler=Draft
    public async Task<IActionResult> OnPostDraftAsync([FromBody] DraftRequest request, CancellationToken cancellationToken)
    {
        var text = request?.Text ?? "";

        // Bad input is rejected before it uses up a draft.
        if (string.IsNullOrWhiteSpace(text))
        {
            return DraftJson(DraftOutcome.Fail(DraftErrorCode.EmptyInput), null);
        }

        if (text.Length > _limits.MaxInputChars)
        {
            return DraftJson(DraftOutcome.Fail(DraftErrorCode.InputTooLong, _limits.MaxInputChars), null);
        }

        var visitor = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var permit = await _limiter.TryConsumeAsync(visitor, cancellationToken);
        if (!permit.Allowed)
        {
            return DraftJson(DraftOutcome.Fail(permit.Code), permit.DraftsLeft);
        }

        var outcome = await _drafter.DraftAsync(text, cancellationToken);
        return DraftJson(outcome, permit.DraftsLeft);
    }

    public async Task<IActionResult> OnPostSaveAsync()
    {
        if (!ModelState.IsValid)
        {
            LoadSamples();
            DraftsLeftToday = await _limiter.GetRemainingAsync();
            ShowReview = true;
            return Page();
        }

        var now = DateTime.UtcNow;
        var record = new DecisionRecord
        {
            Title = Input.Title.Trim(),
            Decision = Input.Decision.Trim(),
            Context = Input.Context?.Trim() ?? "",
            OptionsConsidered = Input.OptionsConsidered?.Trim() ?? "",
            Rationale = Input.Rationale?.Trim() ?? "",
            Consequences = Input.Consequences?.Trim() ?? "",
            AdmPhase = Input.AdmPhase,
            Domain = Input.Domain,
            Owner = string.IsNullOrWhiteSpace(Input.Owner) ? null : Input.Owner.Trim(),
            Status = Input.Status,
            DecisionDate = Input.DecisionDate,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        _db.DecisionRecords.Add(record);
        await _db.SaveChangesAsync();

        TempData["Success"] = "Record saved.";
        return RedirectToPage("/Records/Details", new { id = record.Id });
    }

    private static JsonResult DraftJson(DraftOutcome outcome, int? draftsLeftToday)
    {
        return new JsonResult(new
        {
            ok = outcome.Success,
            errorCode = outcome.ErrorCode.ToString(),
            message = outcome.Message,
            record = outcome.Record,
            note = outcome.Note,
            draftsLeftToday
        });
    }

    private void LoadSamples()
    {
        var samples = SampleDiscussions.All
            .Append(SampleDiscussions.ThinChatTest)
            .DistinctBy(s => s.Number)
            .Select(s => new { number = s.Number, title = s.Title, text = s.Text });

        SamplesJson = JsonSerializer.Serialize(samples, JsonOptions);
    }
}