using DecisionLens.Web.Data;
using DecisionLens.Web.Models;
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
}