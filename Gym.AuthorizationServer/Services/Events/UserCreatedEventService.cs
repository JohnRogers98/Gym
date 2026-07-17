using Gym.AuthorizationServer.Options;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Net.Mime;
using System.Text.Json;

namespace Gym.AuthorizationServer.Services.Events
{
    public interface IUserCreatedEventService
    {
        Task PublishAsync(UserCreatedEvent userCreatedEvent, CancellationToken cancellationToken);
    }

    public class UserCreatedEventService(IConnection _connection, IOptions<RabbitMQOptions> _rabbitOptions) : IUserCreatedEventService
    {
        public async Task PublishAsync(UserCreatedEvent userCreatedEvent, CancellationToken cancellationToken)
        {
            var channelOptions = new CreateChannelOptions(publisherConfirmationsEnabled: true, publisherConfirmationTrackingEnabled: false);

            await using var channel = await _connection.CreateChannelAsync(channelOptions);

            var body = JsonSerializer.SerializeToUtf8Bytes(userCreatedEvent);

            var properties = new BasicProperties
            {
                ContentType = MediaTypeNames.Application.Json,
                DeliveryMode = DeliveryModes.Persistent,
                MessageId = Guid.NewGuid().ToString(),
                Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            };

            await channel.BasicPublishAsync(
                exchange: _rabbitOptions.Value.Exchange,
                routingKey: "user.created",
                mandatory: true,
                basicProperties: properties,
                body: body,
                cancellationToken: cancellationToken
            );
        }
    }

    public record UserCreatedEvent(String UserId, String? FirstName, String? LastName, String Role);
}
