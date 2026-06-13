using ReviewIQ.Shared.Constants;
using ReviewIQ.Shared.Messages;

namespace ReviewIQ.AI.Interfaces;

public interface IReviewOrchestrator
{
    Task<OrchestratorResult> ProcessAsync(PrReviewRequestedMessage message);
}