using DecisionLens.Web.Models;
using DecisionLens.Web.Services;
using Xunit;

namespace DecisionLens.Tests;

public class MarkdownExporterTests
{
    private static DecisionRecord Sample() => new()
    {
        Id = 12,
        Title = "Use PostgreSQL for the Orders service",
        Decision = "Use PostgreSQL.",
        Context = "The team needs a database.",
        OptionsConsidered = "PostgreSQL\nMongoDB",
        Rationale = "Native reporting joins.",
        Consequences = "Indexing discipline is needed.",
        AdmPhase = "C - Information Systems Architectures",
        Domain = "Data",
        Owner = "Anita Rao",
        Status = "Accepted",
        DecisionDate = new DateOnly(2026, 10, 24)
    };

    [Fact]
    public void AllFieldsArePresentAndInOrder()
    {
        var markdown = MarkdownExporter.Build(Sample());

        var order = new[]
        {
            "# Use PostgreSQL for the Orders service",
            "**Status:** Accepted",
            "**Date:** 2026-10-24",
            "**Owner:** Anita Rao",
            "**ADM phase:** C - Information Systems Architectures",
            "**Domain:** Data",
            "## Context",
            "## Decision",
            "## Options considered",
            "## Rationale",
            "## Consequences and risks"
        };

        var lastIndex = -1;
        foreach (var part in order)
        {
            var index = markdown.IndexOf(part, StringComparison.Ordinal);
            Assert.True(index > lastIndex, $"'{part}' is missing or out of order.");
            lastIndex = index;
        }
    }

    [Fact]
    public void SpecialCharactersArePreserved()
    {
        var record = Sample();
        record.Title = "Use C# & <b>JSON</b> \"quotes\" 100%";
        record.Decision = "Ünïcode – dash, ‘quotes’ and emoji-free text.";

        var markdown = MarkdownExporter.Build(record);

        Assert.Contains("# Use C# & <b>JSON</b> \"quotes\" 100%", markdown);
        Assert.Contains("Ünïcode – dash, ‘quotes’ and emoji-free text.", markdown);
    }

    [Fact]
    public void OptionsBecomeBullets_OnePerLine()
    {
        var markdown = MarkdownExporter.Build(Sample());

        Assert.Contains("- PostgreSQL\n- MongoDB", markdown);
    }

    [Fact]
    public void EmptyField_ShowsNotProvided()
    {
        var record = Sample();
        record.Context = "";

        var markdown = MarkdownExporter.Build(record);

        Assert.Contains("## Context\n\nNot provided", markdown);
    }

    [Fact]
    public void MissingOwner_ShowsNotSet()
    {
        var record = Sample();
        record.Owner = null;

        var markdown = MarkdownExporter.Build(record);

        Assert.Contains("**Owner:** Not set", markdown);
    }

    [Fact]
    public void FileName_IsLowerCaseWithHyphens()
    {
        var record = Sample();
        record.Title = "Use PostgreSQL for the Orders service!";

        Assert.Equal("decision-12-use-postgresql-for-the-orders-service.md", MarkdownExporter.FileName(record));
    }

    [Fact]
    public void FileName_FallsBackWhenTitleHasNoLettersOrDigits()
    {
        var record = Sample();
        record.Id = 5;
        record.Title = "!!! ???";

        Assert.Equal("decision-5-record.md", MarkdownExporter.FileName(record));
    }

    [Fact]
    public void FileName_SlugIsCutAt60Characters_WithoutTrailingHyphen()
    {
        var record = Sample();
        record.Title = string.Join(" ", Enumerable.Repeat("architecture", 20));

        var name = MarkdownExporter.FileName(record);
        var slug = name.Replace("decision-12-", "").Replace(".md", "");

        Assert.True(slug.Length <= 60);
        Assert.False(slug.EndsWith("-"));
    }
}