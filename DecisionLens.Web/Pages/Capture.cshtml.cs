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
    private readonly LimitsOptions _limits;

    public CaptureModel(IDecisionDrafter drafter, AppDbContext db, IOptions<LimitsOptions> limits)
    {
        _drafter = drafter;
        _db = db;
        _limits = limits.Value;
    }

    [BindProperty]
    public RecordInput Input { get; set; } = new();

    public bool ShowReview { get; private set; }

    public int MaxInputChars => _limits.MaxInputChars;

    // JSON for the sample dropdown, read by capture.js
    public string SamplesJson { get; private set; } = "[]";

    public void OnGet()
    {
        LoadSamples();
    }

    // Called by capture.js with fetch: POST /Capture?handler=Draft
    public async Task<IActionResult> OnPostDraftAsync([FromBody] DraftRequest request, CancellationToken cancellationToken)
    {
        var outcome = await _drafter.DraftAsync(request?.Text ?? "", cancellationToken);

        return new JsonResult(new
        {
            ok = outcome.Success,
            errorCode = outcome.ErrorCode.ToString(),
            message = outcome.Message,
            record = outcome.Record,
            note = outcome.Note
        });
    }

    public async Task<IActionResult> OnPostSaveAsync()
    {
        if (!ModelState.IsValid)
        {
            LoadSamples();
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

    private void LoadSamples()
    {
        var samples = SampleDiscussions.All
            .Append(SampleDiscussions.ThinChatTest)
            .DistinctBy(s => s.Number)
            .Select(s => new { number = s.Number, title = s.Title, text = s.Text });

        SamplesJson = JsonSerializer.Serialize(samples, JsonOptions);
    }
}