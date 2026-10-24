using System.Text;
using DecisionLens.Web.Data;
using DecisionLens.Web.Models;
using DecisionLens.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DecisionLens.Web.Pages.Records;

public class DetailsModel : PageModel
{
    private readonly AppDbContext _db;

    public DetailsModel(AppDbContext db)
    {
        _db = db;
    }

    public DecisionRecord Record { get; private set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var record = await _db.DecisionRecords.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
        if (record == null)
        {
            return NotFound();
        }

        Record = record;
        return Page();
    }

    // /Records/Details/12?handler=Markdown
    public async Task<IActionResult> OnGetMarkdownAsync(int id)
    {
        var record = await _db.DecisionRecords.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
        if (record == null)
        {
            return NotFound();
        }

        var bytes = new UTF8Encoding(false).GetBytes(MarkdownExporter.Build(record));
        return File(bytes, "text/markdown; charset=utf-8", MarkdownExporter.FileName(record));
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var record = await _db.DecisionRecords.FirstOrDefaultAsync(r => r.Id == id);
        if (record == null)
        {
            return NotFound();
        }

        _db.DecisionRecords.Remove(record);
        await _db.SaveChangesAsync();

        TempData["Success"] = "Record deleted.";
        return RedirectToPage("/Records/Index");
    }
}