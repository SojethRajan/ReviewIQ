using ReviewIQ.Gateway.Domain;

namespace ReviewIQ.Gateway.Interfaces
{
    public interface IWebhookPublisher
    {
        Task PublishAsync(IncomingEvent incomingEvent, string commitSha, string repositoryOwner, string repositoryName);
    }
}
