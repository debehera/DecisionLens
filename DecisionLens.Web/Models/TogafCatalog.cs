namespace DecisionLens.Web.Models;

public static class TogafCatalog
{
    public const string NotStated = "Not stated in the discussion";

    public static readonly IReadOnlyList<string> AdmPhases = new[]
    {
        "Preliminary",
        "A - Architecture Vision",
        "B - Business Architecture",
        "C - Information Systems Architectures",
        "D - Technology Architecture",
        "E - Opportunities and Solutions",
        "F - Migration Planning",
        "G - Implementation Governance",
        "H - Architecture Change Management",
        "Requirements Management"
    };

    public static readonly IReadOnlyList<string> Domains = new[]
    {
        "Business", "Data", "Application", "Technology"
    };

    public static readonly IReadOnlyList<string> Statuses = new[]
    {
        "Proposed", "Accepted", "Rejected", "Superseded"
    };

    private static string Normalize(string? s) =>
        string.Join(' ', (s ?? "")
            .Replace('\u2013', '-')
            .Replace('\u2014', '-')
            .Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));

    private static string? MatchFrom(IReadOnlyList<string> list, string? input)
    {
        var n = Normalize(input);
        if (n.Length == 0) return null;
        return list.FirstOrDefault(v => string.Equals(v, n, StringComparison.OrdinalIgnoreCase));
    }

    public static string? MatchDomain(string? input) => MatchFrom(Domains, input);

    public static string? MatchStatus(string? input) => MatchFrom(Statuses, input);

    // Accepts the exact value, ignoring case and spacing, a bare letter such as "C",
    // "Phase C", or a value that starts with "C - ".
    public static string? MatchPhase(string? input)
    {
        var n = Normalize(input);
        if (n.StartsWith("Phase ", StringComparison.OrdinalIgnoreCase))
        {
            n = n.Substring(6).Trim();
        }
        if (n.Length == 0) return null;

        var exact = MatchFrom(AdmPhases, n);
        if (exact != null) return exact;

        if (n.Length == 1 && char.IsLetter(n[0])) return MatchLetter(n[0]);
        if (n.Length > 3 && n[1] == ' ' && n[2] == '-' && char.IsLetter(n[0])) return MatchLetter(n[0]);

        return null;
    }

    private static string? MatchLetter(char letter) =>
        AdmPhases.FirstOrDefault(p => p.Length > 4 && p[1] == ' ' && p[2] == '-' &&
            char.ToUpperInvariant(p[0]) == char.ToUpperInvariant(letter));
}