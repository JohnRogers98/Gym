using Gym.AuthorizationServer.Admin.Application;
using RabbitMQ.Client;

namespace Gym.AuthorizationServer.Admin.HostedServices
{
    public class RabbitMQTopologyInitializer(IConnection _connection, RabbitMQExchange _rabbitMQExchange) : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await using var channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await this.DeclareExchange(channel, cancellationToken);
            await this.DeclareDeadLetterExchange(channel, cancellationToken);
        }
        private async Task DeclareExchange(IChannel channel, CancellationToken cancellationToken)
        {
            await channel.ExchangeDeclareAsync(
                exchange: _rabbitMQExchange.Value,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false,
                arguments: new Dictionary<String, Object?>
                {
                    { "alternate-exchange", $"{_rabbitMQExchange.Value}-dlx" }
                },
                cancellationToken: cancellationToken
            );
        }
        private async Task DeclareDeadLetterExchange(IChannel channel, CancellationToken cancellationToken)
        {
            await channel.ExchangeDeclareAsync(
            
                exchange: $"{_rabbitMQExchange.Value}-dlx",
                type: ExchangeType.Fanout,
                durable: true,
                autoDelete: false,
                cancellationToken: cancellationToken
            );

            await channel.QueueDeclareAsync(
                queue: $"{_rabbitMQExchange.Value}-dlq",
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
                queue: $"{_rabbitMQExchange.Value}-dlq",
                exchange: $"{_rabbitMQExchange.Value}-dlx",
                routingKey: "#",
                cancellationToken: cancellationToken
            );
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
