using Gym.RabbitMQ.Topology.Messages;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Net.Mime;
using System.Text.Json;

namespace Gym.RabbitMQ.Topology.Services
{
    public interface IUserCreatedEventService
    {
        Task PublishAsync(UserCreatedMessage message, CancellationToken cancellationToken);
    }

    internal class UserCreatedEventService(IConnection _connection, IOptions<RabbitMQOptions> _options) : IUserCreatedEventService
    {
        public async Task PublishAsync(UserCreatedMessage message, CancellationToken cancellationToken)
        {
            var channelOptions = new CreateChannelOptions(publisherConfirmationsEnabled: true, publisherConfirmationTrackingEnabled: false);
            await using var channel = await _connection.CreateChannelAsync(channelOptions, cancellationToken);

            var body = JsonSerializer.SerializeToUtf8Bytes(message);

            var properties = new BasicProperties
            {
                ContentType = MediaTypeNames.Application.Json,
                DeliveryMode = DeliveryModes.Persistent,
                MessageId = Guid.NewGuid().ToString(),
                Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            };

            await channel.BasicPublishAsync(
                exchange: _options.Value.AuthorizationServerExchange,
                routingKey: RoutingKeys.User.Created,
                mandatory: true,
                basicProperties: properties,
                body: body,
                cancellationToken: cancellationToken
            );
        }
    }
}
