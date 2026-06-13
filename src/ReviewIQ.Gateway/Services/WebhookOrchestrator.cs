using Microsoft.EntityFrameworkCore;
using ReviewIQ.Gateway.Domain;
using ReviewIQ.Gateway.Infrastructure;
using ReviewIQ.Gateway.Interfaces;
using ReviewIQ.Shared.Constants;

namespace ReviewIQ.Gateway.Services
{
    public class WebhookOrchestrator : IWebhookOrchestrator
    {
        private readonly IHmacValidationService _hmacValidationService;
        private readonly IGitHubPayloadParser _payloadParser;
        private readonly IWebhookPublisher _webhookPublisher;
        private readonly GatewayDbContext _context;
        private readonly ILogger<WebhookOrchestrator> _logger;

        public WebhookOrchestrator(
            IHmacValidationService hmacValidationService,
            IGitHubPayloadParser gitHubPayloadParser,
            IWebhookPublisher webhookPublisher,
            GatewayDbContext context,
            ILogger<WebhookOrchestrator> logger
            )
        {
            _payloadParser = gitHubPayloadParser;
            _hmacValidationService = hmacValidationService;
            _webhookPublisher = webhookPublisher;
            _context = context;
            _logger = logger;
        }

        public async Task<OrchestratorResult> HandleAsync(string rawBody, string signatureHeader, string deliveryId, string eventType)
        {

            if (!_hmacValidationService.IsValid(signatureHeader, rawBody))
            {
                _logger.LogWarning("HMAC validation failed for DeliveryId {DeliveryId}", deliveryId);
                return OrchestratorResult.InvalidSignature;
            }

            if (eventType != "pull_request")
            {
                _logger.LogInformation("Ignoring event type {EventType}", eventType);
                return OrchestratorResult.EventIgnored;
            }


            var payload = _payloadParser.Parse(rawBody);

            if (payload.Action != "opened" && payload.Action != "synchronize")
            {
                _logger.LogInformation("Ignoring pull_request action {Action}", payload.Action);
                return OrchestratorResult.ActionIgnored;
            }

            var alreadyProcessed = await _context.IncomingEvents.AnyAsync(e => e.DeliveryId == deliveryId);

            if (alreadyProcessed)
            {
                _logger.LogWarning("Duplicate DeliveryId {DeliveryId} — ignoring.", deliveryId);
                return OrchestratorResult.DuplicateEvent;
            }


            var repository = await _context.Repositories
                .FirstOrDefaultAsync(r =>
                    r.Owner == payload.RepositoryOwner &&
                    r.Name == payload.RepositoryName);

            if (repository == null)
            {
                _logger.LogWarning(
                    "Repository {Owner}/{Name} not found.",
                    payload.RepositoryOwner,
                    payload.RepositoryName);
                return OrchestratorResult.RepositoryNotFound;
            }

            var incomingEvent = IncomingEvent.Create(
                repositoryId: repository.Id,
                deliveryId: deliveryId,
                eventType: eventType,
                action: payload.Action,
                pullRequestNumber: payload.PullRequestNumber,
                pullRequestTitle: payload.PullRequestTitle,
                prAuthorLogin: payload.PrAuthorLogin,
                rawPayload: payload.RawBody);

            _context.IncomingEvents.Add(incomingEvent);

            try
            {
                await _context.SaveChangesAsync();

                await _webhookPublisher.PublishAsync(
                    incomingEvent,
                    payload.CommitSha,
                    payload.RepositoryOwner,
                    payload.RepositoryName);

                incomingEvent.MarkAsPublished();
                await _context.SaveChangesAsync();

                _logger.LogInformation(
                    "Successfully processed DeliveryId {DeliveryId}, PR #{PullRequestNumber}",
                    deliveryId,
                    payload.PullRequestNumber);

                return OrchestratorResult.Success;
            }
            catch (Exception ex)
            {
                incomingEvent.MarkAsFailed();
                await _context.SaveChangesAsync();
                // Fallback for any other publish-related exceptions; logs exact exception type
                _logger.LogError(ex, "Failed to publish DeliveryId {DeliveryId}. Exception type: {Type}", deliveryId, ex.GetType().Name);
                return OrchestratorResult.PublishFailed;
            }
        }
    }
}
