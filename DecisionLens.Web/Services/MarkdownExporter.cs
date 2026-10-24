using System.Text;
using DecisionLens.Web.Models;

namespace DecisionLens.Web.Services;

public static class MarkdownExporter
{
    public static string Build(DecisionRecord record)
    {
        var sb = new StringBuilder();

        sb.Append("# ").Append(Clean(record.Title)).Append("\n\n");

        sb.Append("- **Status:** ").Append(record.Status).Append('\n');
        sb.Append("- **Date:** ").Append(record.DecisionDate.ToString("yyyy-MM-dd")).Append('\n');
        sb.Append("- **Owner:** ").Append(string.IsNullOrWhiteSpace(record.Owner) ? "Not set" : Clean(record.Owner)).Append('\n');
        sb.Append("- **ADM phase:** ").Append(record.AdmPhase).Append('\n');
        sb.Append("- **Domain:** ").Append(record.Domain).Append("\n\n");

        AppendSection(sb, "Context", record.Context);
        AppendSection(sb, "Decision", record.Decision);
        AppendOptions(sb, record.OptionsConsidered);
        AppendSection(sb, "Rationale", record.Rationale);
        AppendSection(sb, "Consequences and risks", record.Consequences);

        return sb.ToString();
    }

    // Example: decision-12-use-postgresql-for-the-orders-service.md
    public static string FileName(DecisionRecord record)
    {
        var sb = new StringBuilder();
        var lastWasDash = false;

        foreach (var ch in record.Title.ToLowerInvariant())
        {
            if (ch is (>= 'a' and <= 'z') or (>= '0' and <= '9'))
            {
                sb.Append(ch);
                lastWasDash = false;
            }
            else if (!lastWasDash && sb.Length > 0)
            {
                sb.Append('-');
                lastWasDash = true;
            }
        }

        var slug = sb.ToString().Trim('-');
        if (slug.Length > 60)
        {
            slug = slug[..60].Trim('-');
        }

        if (slug.Length == 0)
        {
            slug = "record";
        }

        return $"decision-{record.Id}-{slug}.md";
    }

    private static void AppendSection(StringBuilder sb, string heading, string? text)
    {
        sb.Append("## ").Append(heading).Append("\n\n");
        sb.Append(string.IsNullOrWhiteSpace(text) ? "Not provided" : Normalize(text)).Append("\n\n");
    }

    // One option per line becomes one bullet per line.
    private static void AppendOptions(StringBuilder sb, string? text)
    {
        sb.Append("## Options considered\n\n");

        if (string.IsNullOrWhiteSpace(text))
        {
            sb.Append("Not provided\n\n");
            return;
        }

        var lines = Normalize(text).Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        foreach (var line in lines)
        {
            sb.Append(line.StartsWith("- ") ? line : "- " + line).Append('\n');
        }

        sb.Append('\n');
    }

    private static string Normalize(string text) => text.Replace("\r\n", "\n").Replace('\r', '\n').Trim();

    private static string Clean(string text) => Normalize(text).Replace('\n', ' ');
}