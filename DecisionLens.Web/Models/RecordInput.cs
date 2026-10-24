using System.ComponentModel.DataAnnotations;

namespace DecisionLens.Web.Models;

public class RecordInput : IValidatableObject
{
    [Required(ErrorMessage = "Enter a title."), StringLength(150)]
    public string Title { get; set; } = "";

    [Required(ErrorMessage = "Enter the decision."), StringLength(4000)]
    public string Decision { get; set; } = "";

    [StringLength(4000)]
    public string? Context { get; set; }

    [Display(Name = "Options considered"), StringLength(4000)]
    public string? OptionsConsidered { get; set; }

    [StringLength(4000)]
    public string? Rationale { get; set; }

    [Display(Name = "Consequences and risks"), StringLength(4000)]
    public string? Consequences { get; set; }

    [Display(Name = "ADM phase"), Required(ErrorMessage = "Choose an ADM phase.")]
    public string AdmPhase { get; set; } = "";

    [Display(Name = "Architecture domain"), Required(ErrorMessage = "Choose a domain.")]
    public string Domain { get; set; } = "";

    [StringLength(100)]
    public string? Owner { get; set; }

    [Required(ErrorMessage = "Choose a status.")]
    public string Status { get; set; } = "Proposed";

    [Display(Name = "Decision date")]
    public DateOnly DecisionDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    // Runs automatically during model binding, after the attribute checks.
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!string.IsNullOrEmpty(AdmPhase) && !TogafCatalog.AdmPhases.Contains(AdmPhase))
        {
            yield return new ValidationResult("Choose one of the listed ADM phases.", new[] { nameof(AdmPhase) });
        }

        if (!string.IsNullOrEmpty(Domain) && !TogafCatalog.Domains.Contains(Domain))
        {
            yield return new ValidationResult("Choose one of the listed domains.", new[] { nameof(Domain) });
        }

        if (!string.IsNullOrEmpty(Status) && !TogafCatalog.Statuses.Contains(Status))
        {
            yield return new ValidationResult("Choose one of the listed statuses.", new[] { nameof(Status) });
        }
    }
}