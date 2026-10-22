namespace DecisionLens.Web.Services;

public enum DraftErrorCode
{
    None,
    EmptyInput,
    InputTooLong,
    Cooldown,
    DailyLimit,
    ProviderRateLimit,
    ProviderTimeout,
    InvalidReply,
    NotConfigured,
    ProviderError
}

public record DraftedRecord(
    string Title,
    string Decision,
    string Context,
    string OptionsConsidered,
    string Rationale,
    string Consequences,
    string AdmPhase,
    string Domain);

public record DraftNote(string Confidence, string Reason);

public class DraftOutcome
{
    public bool Success { get; init; }
    public DraftedRecord? Record { get; init; }
    public DraftNote? Note { get; init; }
    public DraftErrorCode ErrorCode { get; init; } = DraftErrorCode.None;
    public string Message { get; init; } = "";

    public static DraftOutcome Ok(DraftedRecord record, DraftNote note) =>
        new() { Success = true, Record = record, Note = note };

    public static DraftOutcome Fail(DraftErrorCode code, int maxChars = 0) =>
        new() { Success = false, ErrorCode = code, Message = DraftMessages.Get(code, maxChars) };
}

public static class DraftMessages
{
    public static string Get(DraftErrorCode code, int maxChars = 0) => code switch
    {
        DraftErrorCode.EmptyInput => "Paste a discussion first, or load a sample.",
        DraftErrorCode.InputTooLong => $"That is over {maxChars:N0} characters. Trim it to the part that contains the decision.",
        DraftErrorCode.Cooldown => "Please wait a few seconds before drafting again.",
        DraftErrorCode.DailyLimit => "The free daily AI limit for this demo has been reached. You can still create a record manually, or try again tomorrow.",
        DraftErrorCode.ProviderRateLimit => "The free AI service is busy right now. Wait a minute and try again, or create the record manually.",
        DraftErrorCode.ProviderTimeout => "The AI took too long to respond. Try again, or create the record manually.",
        DraftErrorCode.InvalidReply => "The AI's answer could not be turned into a record. Try again, or edit the text and retry.",
        DraftErrorCode.NotConfigured => "The AI service is not configured on this server. You can still create a record manually.",
        DraftErrorCode.ProviderError => "The AI service had a problem. Try again later or create the record manually.",
        _ => ""
    };
}