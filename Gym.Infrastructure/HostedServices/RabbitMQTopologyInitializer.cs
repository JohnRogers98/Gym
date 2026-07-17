using Gym.Infrastructure.Configurations;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace Gym.Infrastructure.HostedServices
{
    public class RabbitMQTopologyInitializer(IConnection _connection, RabbitMQOptions _rabbitMQOptions) : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await using var channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await this.DeclareDeadLetterExchange(channel, cancellationToken);
            await this.DeclareExchange(channel, cancellationToken);
        }
        private async Task DeclareExchange(IChannel channel, CancellationToken cancellationToken)
        {
            await channel.ExchangeDeclareAsync(
                exchange: _rabbitMQOptions.Exchange,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false,
                arguments: new Dictionary<String, Object?>
                {
                    { "alternate-exchange", $"{_rabbitMQOptions.Exchange}-dlx" }
                },
                cancellationToken: cancellationToken
            );

            await channel.QueueDeclareAsync(
                queue: $"created-users-worker-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: new Dictionary<String, Object?>
                {
                    { "x-message-ttl", 86400000 },
                    { "x-queue-type", "quorum" },
                    { "x-dead-letter-exchange", $"{_rabbitMQOptions.Exchange}-dlx" },
                    { "x-dead-letter-routing-key", "user.created" }
                },
                cancellationToken: cancellationToken
            );

            await channel.QueueBindAsync(
                queue: $"created-users-worker-queue",
                exchange: _rabbitMQOptions.Exchange,
                routingKey: "user.created",
                cancellationToken: cancellationToken
            );
        }
        private async Task DeclareDeadLetterExchange(IChannel channel, CancellationToken cancellationToken)
        {
            await channel.ExchangeDeclareAsync(
            
                exchange: $"{_rabbitMQOptions.Exchange}-dlx",
                type: ExchangeType.Fanout,
                durable: true,
                autoDelete: false,
                cancellationToken: cancellationToken
            );

            await channel.QueueDeclareAsync(
                queue: "created-users-worker-queue-dlq",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: new Dictionary<String, Object?>
                {
                    { "x-message-ttl", 86400000 },
                    { "x-queue-type", "quorum" }
                },
                cancellationToken: cancellationToken
            );

            await channel.QueueBindAsync(
                queue: $"created-users-worker-queue-dlq",
                exchange: $"{_rabbitMQOptions.Exchange}-dlx",
                routingKey: String.Empty,
                cancellationToken: cancellationToken
            );
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
