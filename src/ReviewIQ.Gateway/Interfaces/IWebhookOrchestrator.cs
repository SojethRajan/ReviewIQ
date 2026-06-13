using ReviewIQ.Shared.Constants;

namespace ReviewIQ.Gateway.Interfaces
{
    public interface IWebhookOrchestrator
    {
        Task<OrchestratorResult> HandleAsync(
        string rawBody,
        string signatureHeader,
        string deliveryId,
        string eventType);
    }
}
