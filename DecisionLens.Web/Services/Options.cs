namespace DecisionLens.Web.Services;

public class AiOptions
{
    public string Provider { get; set; } = "Gemini";
    public string Model { get; set; } = "";
    public string ApiKey { get; set; } = "";
    public int TimeoutSeconds { get; set; } = 40;
}

public class LimitsOptions
{
    public int MaxInputChars { get; set; } = 6000;
    public int DailyDraftLimit { get; set; } = 40;
    public int CooldownSeconds { get; set; } = 10;
}