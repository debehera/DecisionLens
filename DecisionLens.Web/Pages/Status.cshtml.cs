using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DecisionLens.Web.Pages;

// Shown for 404, 400 and other HTTP errors (see UseStatusCodePagesWithReExecute in Program.cs).
[IgnoreAntiforgeryToken]
public class StatusModel : PageModel
{
    public int Code { get; private set; }

    public string Heading { get; private set; } = "";

    public string Message { get; private set; } = "";

    // A failed POST is re-run as a POST, so both handlers are needed.
    public void OnGet(int? code) => Fill(code);

    public void OnPost(int? code) => Fill(code);

    private void Fill(int? code)
    {
        Code = code ?? 500;

        switch (Code)
        {
            case 404:
                Heading = "We couldn't find that page";
                Message = "The record may have been deleted, or the link may be wrong.";
                break;
            case 400:
                Heading = "That request couldn't be processed";
                Message = "If you had a form open for a long time, go back, refresh the page and try again.";
                break;
            case 429:
                Heading = "Too many requests";
                Message = "Please wait a moment and try again.";
                break;
            default:
                Heading = "Something went wrong";
                Message = "Please try again. If it keeps happening, come back a little later.";
                break;
        }
    }
}