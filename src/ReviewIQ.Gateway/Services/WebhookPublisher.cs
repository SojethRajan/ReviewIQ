using RabbitMQ.Client;
using ReviewIQ.Gateway.Domain;
using ReviewIQ.Gateway.Interfaces;
using ReviewIQ.Shared.Constants;
using ReviewIQ.Shared.Messages;
using System.Text;
using System.Text.Json;

namespace ReviewIQ.Gateway.Services
{
    public class WebhookPublisher : IWebhookPublisher
    {
        private readonly IConnection _connection;
        private readonly ILogger<WebhookPublisher> _logger;

        public WebhookPublisher(IConnection connection, ILogger<WebhookPublisher> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public async Task PublishAsync(IncomingEvent incomingEvent, string commitSha, string repositoryOwner, string repositoryName)
        {
            await using var channel = await _connection.CreateChannelAsync();

            var message = new PrReviewRequestedMessage
            {
                IncomingEventId = incomingEvent.Id,
                RepositoryId = incomingEvent.RepositoryId,
                DeliveryId = incomingEvent.DeliveryId,
                RepositoryOwner = repositoryOwner,
                RepositoryName = repositoryName,
                PullRequestNumber = incomingEvent.PullRequestNumber,
                PullRequestTitle = incomingEvent.PullRequestTitle,
                PrAuthorLogin = incomingEvent.PrAuthorLogin,
                CommitSha = commitSha,
                ReceivedOn = incomingEvent.ReceivedOn
            };

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            var properties = new BasicProperties
            {
                Persistent = true,
                ContentType = "application/json"
            };

            await channel.BasicPublishAsync(
                exchange: QueueNames.PrReviewExchange,
                routingKey: QueueNames.PrReviewQueue,
                mandatory: false,
                basicProperties: properties,
                body: body);

            _logger.LogInformation(
                "Published PrReviewRequestedMessage for DeliveryId {DeliveryId}, PR #{PullRequestNumber}",
                incomingEvent.DeliveryId,
                incomingEvent.PullRequestNumber);
        }
    }
}
