using ReviewIQ.Shared.Messages;

namespace ReviewIQ.AI.Interfaces;

public interface IReviewPublisher
{
    Task PublishAsync(ReviewCompletedMessage message);
}