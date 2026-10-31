using System.Text.Json;
using DecisionLens.Web.Models;
using DecisionLens.Web.Services;
using Xunit;

namespace DecisionLens.Tests;

public class DraftValidatorTests
{
    private static Dictionary<string, string> ValidFields() => new()
    {
        ["title"] = "Use PostgreSQL for the Orders service",
        ["decision"] = "The Orders service will use PostgreSQL.",
        ["context"] = "The team needs a database.",
        ["options_considered"] = "PostgreSQL - Pros: joins; Cons: schema changes",
        ["rationale"] = "Native reporting joins.",
        ["consequences"] = "Indexing discipline is needed.",
        ["adm_phase"] = "C - Information Systems Architectures",
        ["domain"] = "Data",
        ["tag_confidence"] = "high",
        ["tag_reason"] = "Database choice."
    };

    private static string ToJson(Dictionary<string, string> fields) => JsonSerializer.Serialize(fields);

    [Fact]
    public void ValidJson_Passes()
    {
        var outcome = DraftValidator.Validate(ToJson(ValidFields()));

        Assert.True(outcome.Success);
        Assert.Equal("Use PostgreSQL for the Orders service", outcome.Record!.Title);
        Assert.Equal("C - Information Systems Architectures", outcome.Record.AdmPhase);
        Assert.Equal("Data", outcome.Record.Domain);
        Assert.Equal("high", outcome.Note!.Confidence);
    }

    [Fact]
    public void JsonInsideCodeFences_Passes()
    {
        var fenced = "```json\n" + ToJson(ValidFields()) + "\n```";

        var outcome = DraftValidator.Validate(fenced);

        Assert.True(outcome.Success);
    }

    [Fact]
    public void MissingProperty_Fails()
    {
        var fields = ValidFields();
        fields.Remove("rationale");

        var outcome = DraftValidator.Validate(ToJson(fields));

        Assert.False(outcome.Success);
        Assert.Equal(DraftErrorCode.InvalidReply, outcome.ErrorCode);
    }

    [Fact]
    public void UnknownPhase_Fails()
    {
        var fields = ValidFields();
        fields["adm_phase"] = "Banana Architecture";

        var outcome = DraftValidator.Validate(ToJson(fields));

        Assert.False(outcome.Success);
        Assert.Equal(DraftErrorCode.InvalidReply, outcome.ErrorCode);
    }

    [Fact]
    public void BareLetterPhase_MapsToFullValue()
    {
        var fields = ValidFields();
        fields["adm_phase"] = "C";

        var outcome = DraftValidator.Validate(ToJson(fields));

        Assert.True(outcome.Success);
        Assert.Equal("C - Information Systems Architectures", outcome.Record!.AdmPhase);
    }

    [Fact]
    public void DomainIsMatchedIgnoringCase()
    {
        var fields = ValidFields();
        fields["domain"] = "  tECHNOLOGY ";

        var outcome = DraftValidator.Validate(ToJson(fields));

        Assert.True(outcome.Success);
        Assert.Equal("Technology", outcome.Record!.Domain);
    }

    [Fact]
    public void EmptyField_BecomesPlaceholder()
    {
        var fields = ValidFields();
        fields["rationale"] = "   ";

        var outcome = DraftValidator.Validate(ToJson(fields));

        Assert.True(outcome.Success);
        Assert.Equal(TogafCatalog.NotStated, outcome.Record!.Rationale);
    }

    [Fact]
    public void LongTitle_IsCutAt150Characters()
    {
        var fields = ValidFields();
        fields["title"] = new string('a', 200);

        var outcome = DraftValidator.Validate(ToJson(fields));

        Assert.True(outcome.Success);
        Assert.Equal(150, outcome.Record!.Title.Length);
    }

    [Fact]
    public void InvalidConfidence_BecomesLow()
    {
        var fields = ValidFields();
        fields["tag_confidence"] = "very sure";

        var outcome = DraftValidator.Validate(ToJson(fields));

        Assert.True(outcome.Success);
        Assert.Equal("low", outcome.Note!.Confidence);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("this is not json")]
    [InlineData("[1, 2, 3]")]
    public void EmptyOrNotAnObject_Fails(string? text)
    {
        var outcome = DraftValidator.Validate(text);

        Assert.False(outcome.Success);
        Assert.Equal(DraftErrorCode.InvalidReply, outcome.ErrorCode);
    }
}