using System.ComponentModel.DataAnnotations;

namespace DecisionLens.Web.Models;

public class DecisionRecord
{
    public int Id { get; set; }

    [Required, MaxLength(150)]
    public string Title { get; set; } = "";

    [Required, MaxLength(4000)]
    public string Decision { get; set; } = "";

    [MaxLength(4000)]
    public string Context { get; set; } = "";

    // One option per line
    [MaxLength(4000)]
    public string OptionsConsidered { get; set; } = "";

    [MaxLength(4000)]
    public string Rationale { get; set; } = "";

    [MaxLength(4000)]
    public string Consequences { get; set; } = "";

    [Required, MaxLength(60)]
    public string AdmPhase { get; set; } = "";

    [Required, MaxLength(20)]
    public string Domain { get; set; } = "";

    [MaxLength(100)]
    public string? Owner { get; set; }

    [Required, MaxLength(20)]
    public string Status { get; set; } = "Proposed";

    public DateOnly DecisionDate { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
}