using DecisionLens.Web.Data;
using DecisionLens.Web.Models;
using Microsoft.AspNetCore.Mvc;
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

    // Bound from the query string: /Records?phase=...&domain=...
    [BindProperty(SupportsGet = true)]
    public string? Phase { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Domain { get; set; }

    public List<DecisionRecord> Records { get; private set; } = new();

    public int TotalCount { get; private set; }

    public bool HasFilters => Phase != null || Domain != null;

    public async Task OnGetAsync()
    {
        // Ignore any value that is not in the catalog.
        if (Phase != null && !TogafCatalog.AdmPhases.Contains(Phase))
        {
            Phase = null;
        }

        if (Domain != null && !TogafCatalog.Domains.Contains(Domain))
        {
            Domain = null;
        }

        IQueryable<DecisionRecord> query = _db.DecisionRecords.AsNoTracking();

        if (Phase != null)
        {
            query = query.Where(r => r.AdmPhase == Phase);
        }

        if (Domain != null)
        {
            query = query.Where(r => r.Domain == Domain);
        }

        Records = await query
            .OrderByDescending(r => r.DecisionDate)
            .ThenByDescending(r => r.Id)
            .ToListAsync();

        TotalCount = await _db.DecisionRecords.CountAsync();
    }
}