using MediatR;
using RabbitMQ.Client;
using System.Net.Mime;
using System.Text.Json;

namespace Gym.AuthorizationServer.Admin.Application.Services.UserApi.CreateUser
{
    internal class UserCreatedNotificationHandler(IConnection _connection, RabbitMQExchange rabbitMQExchange) : INotificationHandler<UserCreatedNotification>
    {
        public async Task Handle(UserCreatedNotification notification, CancellationToken cancellationToken)
        {
            var channelOptions = new CreateChannelOptions(publisherConfirmationsEnabled: true, publisherConfirmationTrackingEnabled: false);

            await using var channel = await _connection.CreateChannelAsync(channelOptions);

            var body = JsonSerializer.SerializeToUtf8Bytes(notification);

            var properties = new BasicProperties
            {
                ContentType = MediaTypeNames.Application.Json,
                DeliveryMode = DeliveryModes.Persistent,
                MessageId = Guid.NewGuid().ToString(),
                Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            };

            await channel.BasicPublishAsync(
                exchange: rabbitMQExchange.Value,
                routingKey: "user.created",
                mandatory: true,
                basicProperties: properties,
                body: body,
                cancellationToken: cancellationToken
            );
        }
    }
}
