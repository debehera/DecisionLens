using DecisionLens.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace DecisionLens.Web.Pages;

public class CaptureModel : PageModel
{
    private readonly IDecisionDrafter _drafter;
    private readonly LimitsOptions _limits;

    public CaptureModel(IDecisionDrafter drafter, IOptions<LimitsOptions> limits)
    {
        _drafter = drafter;
        _limits = limits.Value;
    }

    [BindProperty]
    public string? Discussion { get; set; }

    public DraftOutcome? Outcome { get; private set; }

    public int MaxChars => _limits.MaxInputChars;

    public void OnGet(int? sample)
    {
        if (sample.HasValue)
        {
            Discussion = SampleDiscussions.Get(sample.Value)?.Text;
        }
    }

    public async Task OnPostAsync(CancellationToken cancellationToken)
    {
        Outcome = await _drafter.DraftAsync(Discussion ?? "", cancellationToken);
    }
}