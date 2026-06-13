using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ReviewIQ.AI.Interfaces;
using ReviewIQ.Shared.Constants;
using ReviewIQ.Shared.Messages;

namespace ReviewIQ.AI.Workers;

public class PrReviewWorker : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<PrReviewWorker> _logger;
    private IChannel? _channel;

    public PrReviewWorker(
        IConnection connection,
        IServiceScopeFactory scopeFactory,
        ILogger<PrReviewWorker> logger)
    {
        _connection = connection;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        _channel = await _connection.CreateChannelAsync();

        await _channel.BasicQosAsync(
            prefetchSize: 0,
            prefetchCount: 1,
            global: false);

        _logger.LogInformation("PrReviewWorker started. Listening on {Queue}", QueueNames.PrReviewQueue);

        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel!);

        consumer.ReceivedAsync += async (sender, ea) =>
        {
            var body = ea.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);

            _logger.LogInformation("Received message from {Queue}", QueueNames.PrReviewQueue);

            try
            {
                var message = JsonSerializer.Deserialize<PrReviewRequestedMessage>(json);

                if (message == null)
                {
                    _logger.LogError("Failed to deserialise message. Rejecting.");
                    await _channel!.BasicRejectAsync(ea.DeliveryTag, requeue: false);
                    return;
                }

                using var scope = _scopeFactory.CreateScope();
                var orchestrator = scope.ServiceProvider
                    .GetRequiredService<IReviewOrchestrator>();

                var result = await orchestrator.ProcessAsync(message);

                if (result == OrchestratorResult.Success)
                {
                    await _channel!.BasicAckAsync(ea.DeliveryTag, multiple: false);
                    _logger.LogInformation("Message acknowledged for PR #{PullRequestNumber}",
                        message.PullRequestNumber);
                }
                else
                {
                    _logger.LogWarning("Processing failed with result {Result}. Rejecting message.", result);
                    await _channel!.BasicRejectAsync(ea.DeliveryTag, requeue: false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception processing message. Rejecting.");
                await _channel!.BasicRejectAsync(ea.DeliveryTag, requeue: false);
            }
        };

        await _channel!.BasicConsumeAsync(
            queue: QueueNames.PrReviewQueue,
            autoAck: false,
            consumer: consumer);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel != null)
        {
            await _channel.CloseAsync();
            _channel.Dispose();
        }

        _logger.LogInformation("PrReviewWorker stopped.");
        await base.StopAsync(cancellationToken);
    }
}