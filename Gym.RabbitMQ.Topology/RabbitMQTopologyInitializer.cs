using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Gym.RabbitMQ.Topology
{
    public interface IRabbitMQTopologyInitializer
    {
        Task DeclareUserEventMessagesExchangeAsync(CancellationToken cancellationToken);
        Task DeclareUserCreatedQueueAsync(CancellationToken cancellationToken);
        Task DeclareUserEventMessagesDeadLetterExchangeAsync(CancellationToken cancellationToken);
        Task DeclareUserEventMessagesDeadLetterQueueAsync(CancellationToken cancellationToken);
    }

    internal class RabbitMQTopologyInitializer(IConnection _connection, IOptions<RabbitMQOptions> _options) : IRabbitMQTopologyInitializer
    { 
        private Int32 DayMilliseconds => (Int32)TimeSpan.FromDays(1).TotalMilliseconds;

        public async Task DeclareUserEventMessagesExchangeAsync(CancellationToken cancellationToken)
        {
            await using var channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await channel.ExchangeDeclareAsync(
                exchange: _options.Value.AuthorizationServerExchange,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false,
                arguments: new Dictionary<String, Object?>
                {
                    { "alternate-exchange", _options.Value.AuthorizationServerDeadLetterExchange }
                },
                cancellationToken: cancellationToken
            );
        }

        public async Task DeclareUserCreatedQueueAsync(CancellationToken cancellationToken)
        {
            await using var channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await channel.QueueDeclareAsync(
                queue: _options.Value.AuthorizationServerUserCreatedQueue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: new Dictionary<String, Object?>
                {
                    { "x-message-ttl",  DayMilliseconds },
                    { "x-queue-type", "quorum" },
                    { "x-dead-letter-exchange", _options.Value.AuthorizationServerDeadLetterExchange},
                    { "x-dead-letter-routing-key", RoutingKeys.User.Created }
                },
                cancellationToken: cancellationToken
            );

            await channel.QueueBindAsync(
                queue: _options.Value.AuthorizationServerUserCreatedQueue,
                exchange: _options.Value.AuthorizationServerExchange,
                routingKey: RoutingKeys.User.Created,
                cancellationToken: cancellationToken
            );
        }

        public async Task DeclareUserEventMessagesDeadLetterExchangeAsync(CancellationToken cancellationToken)
        {
            await using var channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await channel.ExchangeDeclareAsync(
                exchange: _options.Value.AuthorizationServerDeadLetterExchange,
                type: ExchangeType.Fanout,
                durable: true,
                autoDelete: false,
                cancellationToken: cancellationToken
            );
        }

        public async Task DeclareUserEventMessagesDeadLetterQueueAsync(CancellationToken cancellationToken)
        {
            await using var channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await channel.QueueDeclareAsync(
                queue: _options.Value.AuthorizationServerDeadLetterQueue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: new Dictionary<String, Object?>
                {
                    { "x-message-ttl",  DayMilliseconds },
                    { "x-queue-type", "quorum" }
                },
                cancellationToken: cancellationToken
            );

            await channel.QueueBindAsync(
                queue: _options.Value.AuthorizationServerDeadLetterQueue,
                exchange: _options.Value.AuthorizationServerDeadLetterExchange,
                routingKey: String.Empty,
                cancellationToken: cancellationToken
            );
        }

    }
}
