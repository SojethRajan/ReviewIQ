using RabbitMQ.Client;
using ReviewIQ.Shared.Constants;

namespace ReviewIQ.Shared.RabbitMQ
{
    public class QueueDeclarationService
    {
        private readonly IConnection _connection;

        public QueueDeclarationService(IConnection connection)
        {
            _connection = connection;
        }

        public async Task DeclareAllAsync()
        {
            await using var channel = await _connection.CreateChannelAsync();

            //Dead Letter Exchange
            await channel.ExchangeDeclareAsync(
                exchange: QueueNames.DeadLetterExchange,
                type: ExchangeType.Direct,
                durable: true);

            //pr.review.exchange (Direct)
            await channel.ExchangeDeclareAsync(
                exchange: QueueNames.PrReviewExchange,
                type: ExchangeType.Direct,
                durable: true);

            //review.completed.exchange (Fanout)
            await channel.ExchangeDeclareAsync(
                exchange: QueueNames.ReviewCompletedExchange,
                type: ExchangeType.Fanout,
                durable: true);

            //pr.review.queue
            await channel.QueueDeclareAsync(
                queue: QueueNames.PrReviewQueue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: new Dictionary<string, object?>
                {
                    { "x-dead-letter-exchange", QueueNames.DeadLetterExchange },
                    { "x-dead-letter-routing-key", QueueNames.PrReviewDlq }
                });

            await channel.QueueBindAsync(
                queue: QueueNames.PrReviewQueue,
                exchange: QueueNames.PrReviewExchange,
                routingKey: QueueNames.PrReviewQueue);

            //review.completed.analytics.queue
            await channel.QueueDeclareAsync(
                queue: QueueNames.ReviewCompletedAnalyticsQueue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: new Dictionary<string, object?>
                {
                    { "x-dead-letter-exchange", QueueNames.DeadLetterExchange },
                    { "x-dead-letter-routing-key", QueueNames.ReviewCompletedAnalyticsDlq }
                });

            await channel.QueueBindAsync(
                queue: QueueNames.ReviewCompletedAnalyticsQueue,
                exchange: QueueNames.ReviewCompletedExchange,
                routingKey: string.Empty);

            //review.completed.notify.queue
            await channel.QueueDeclareAsync(
                queue: QueueNames.ReviewCompletedNotifyQueue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: new Dictionary<string, object?>
                {
                    { "x-dead-letter-exchange", QueueNames.DeadLetterExchange },
                    { "x-dead-letter-routing-key", QueueNames.ReviewCompletedNotifyDlq }
                });

            await channel.QueueBindAsync(
                queue: QueueNames.ReviewCompletedNotifyQueue,
                exchange: QueueNames.ReviewCompletedExchange,
                routingKey: string.Empty);

            //Dead Letter Queues
            foreach (var dlq in new[]
            {
                QueueNames.PrReviewDlq,
                QueueNames.ReviewCompletedAnalyticsDlq,
                QueueNames.ReviewCompletedNotifyDlq
            })
            {
                await channel.QueueDeclareAsync(
                    queue: dlq,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
            }
        }
    }
}
