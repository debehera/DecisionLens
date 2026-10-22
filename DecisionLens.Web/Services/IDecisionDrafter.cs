namespace DecisionLens.Web.Services;

public interface IDecisionDrafter
{
    Task<DraftOutcome> DraftAsync(string discussion, CancellationToken cancellationToken = default);
}