using DecisionLens.Web.Data;
using DecisionLens.Web.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DecisionLens.Web.Pages.Records;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;

    public IndexModel(AppDbContext db)
    {
        _db = db;
    }

    public List<DecisionRecord> Records { get; private set; } = new();

    public async Task OnGetAsync()
    {
        Records = await _db.DecisionRecords
            .AsNoTracking()
            .OrderByDescending(r => r.DecisionDate)
            .ThenByDescending(r => r.Id)
            .ToListAsync();
    }
}