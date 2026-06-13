using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using ReviewIQ.AI.Interfaces;
using ReviewIQ.Shared.Constants;
using ReviewIQ.Shared.Messages;

public class ReviewPublisher : IReviewPublisher
{
    private readonly IConnection _connection;
    private readonly ILogger<ReviewPublisher> _logger;

    public ReviewPublisher(IConnection connection, ILogger<ReviewPublisher> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    public async Task PublishAsync(ReviewCompletedMessage message)
    {
        await using var channel = await _connection.CreateChannelAsync();

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        var properties = new BasicProperties
        {
            Persistent = true,
            ContentType = "application/json"
        };

        await channel.BasicPublishAsync(
            exchange: QueueNames.ReviewCompletedExchange,
            routingKey: string.Empty,
            mandatory: false,
            basicProperties: properties,
            body: body);

        _logger.LogInformation(
            "Published ReviewCompletedMessage for CodeReviewId {CodeReviewId}",
            message.CodeReviewId);
    }
}