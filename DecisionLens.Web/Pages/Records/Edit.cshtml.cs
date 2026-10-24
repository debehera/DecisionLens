using DecisionLens.Web.Data;
using DecisionLens.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DecisionLens.Web.Pages.Records;

public class EditModel : PageModel
{
    private readonly AppDbContext _db;

    public EditModel(AppDbContext db)
    {
        _db = db;
    }

    [BindProperty]
    public RecordInput Input { get; set; } = new();

    public int Id { get; private set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var record = await _db.DecisionRecords.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
        if (record == null)
        {
            return NotFound();
        }

        Id = id;
        Input = new RecordInput
        {
            Title = record.Title,
            Decision = record.Decision,
            Context = record.Context,
            OptionsConsidered = record.OptionsConsidered,
            Rationale = record.Rationale,
            Consequences = record.Consequences,
            AdmPhase = record.AdmPhase,
            Domain = record.Domain,
            Owner = record.Owner,
            Status = record.Status,
            DecisionDate = record.DecisionDate
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        Id = id;

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var record = await _db.DecisionRecords.FirstOrDefaultAsync(r => r.Id == id);
        if (record == null)
        {
            return NotFound();
        }

        record.Title = Input.Title.Trim();
        record.Decision = Input.Decision.Trim();
        record.Context = Input.Context?.Trim() ?? "";
        record.OptionsConsidered = Input.OptionsConsidered?.Trim() ?? "";
        record.Rationale = Input.Rationale?.Trim() ?? "";
        record.Consequences = Input.Consequences?.Trim() ?? "";
        record.AdmPhase = Input.AdmPhase;
        record.Domain = Input.Domain;
        record.Owner = string.IsNullOrWhiteSpace(Input.Owner) ? null : Input.Owner.Trim();
        record.Status = Input.Status;
        record.DecisionDate = Input.DecisionDate;
        record.UpdatedAtUtc = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        TempData["Success"] = "Changes saved.";
        return RedirectToPage("/Records/Details", new { id });
    }
}