using ReviewIQ.Gateway.Models;

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
