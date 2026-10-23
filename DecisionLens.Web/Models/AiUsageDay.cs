namespace DecisionLens.Web.Models;

// One row per UTC day, counting AI drafts. Used from Day 6.
public class AiUsageDay
{
    public DateOnly Day { get; set; }

    public int Count { get; set; }
}