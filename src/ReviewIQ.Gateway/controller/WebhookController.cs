using Microsoft.AspNetCore.Mvc;
using ReviewIQ.Gateway.Interfaces;
using ReviewIQ.Gateway.Models;

namespace ReviewIQ.Gateway.controller
{
    [Route("api/webhooks")]
    public class WebhookController : Controller
    {
        private readonly IWebhookOrchestrator _orchestrator;

        public WebhookController(IWebhookOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
        }

        [HttpPost("github")]
        public async Task<IActionResult> HandleGitHubWebhook()
        {
            using var reader = new StreamReader(Request.Body);
            var rawBody = await reader.ReadToEndAsync();

            var signatureHeader = Request.Headers["X-Hub-Signature-256"].FirstOrDefault() ?? string.Empty;
            var deliveryId = Request.Headers["X-GitHub-Delivery"].FirstOrDefault() ?? string.Empty;
            var eventType = Request.Headers["X-GitHub-Event"].FirstOrDefault() ?? string.Empty;

            if (
                string.IsNullOrWhiteSpace(signatureHeader) || 
                string.IsNullOrWhiteSpace(deliveryId) ||
                string.IsNullOrWhiteSpace(eventType))
            {
                return BadRequest("Missing required headers.");
            }

            var result = await _orchestrator.HandleAsync(rawBody,signatureHeader,deliveryId,eventType);

            return result switch
            {
                OrchestratorResult.Success => Ok(),
                OrchestratorResult.InvalidSignature => Unauthorized("Invalid signature."),
                OrchestratorResult.EventIgnored => Ok("Event ignored."),
                OrchestratorResult.ActionIgnored => Ok("Action ignored."),
                OrchestratorResult.DuplicateEvent => Ok("Duplicate event ignored."),
                OrchestratorResult.RepositoryNotFound => NotFound("Repository not registered."),
                OrchestratorResult.PersistenceFailed => StatusCode(500, "Failed to save event."),
                OrchestratorResult.PublishFailed => StatusCode(500, "Failed to publish event."),
                _ => StatusCode(500, "Unexpected error.")
            };
        }
    }
}
