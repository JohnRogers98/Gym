using Gym.Application.Services.UserApi.CreateClient;
using Gym.Domain._Common;
using Gym.RabbitMQ.Topology;
using Gym.RabbitMQ.Topology.Messages;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace Gym.Infrastructure.HostedServices
{
    internal class UserCreatedMessagesConsumer(IConnection _connection, IServiceScopeFactory _serviceLocator) : BackgroundService
    {
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await using var channel = await _connection.CreateChannelAsync();

                    var consumer = new AsyncEventingBasicConsumer(channel);
                    consumer.ReceivedAsync += async (sender, ea) =>
                    {
                        await using var scope = _serviceLocator.CreateAsyncScope();
                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                        try
                        {
                            var body = ea.Body.ToArray();
                            var message = JsonSerializer.Deserialize<UserCreatedMessage>(body);
                            if(message is null)
                            {
                                await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false, stoppingToken);
                                return;
                            }

                            CreateUser createUser = new(message.UserId, message.Role, message.FirstName!, message.LastName, message.TelegramId);
                            Result createdUserResult = await mediator.Send(createUser);

                            if (createdUserResult.Success)
                            {
                                await channel.BasicAckAsync(ea.DeliveryTag, multiple: false, stoppingToken);
                            }
                            else
                            {
                                await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false, stoppingToken);
                            }
                        }
                        catch
                        {
                            await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false, stoppingToken);
                        }
                    };

                    await using var scope = _serviceLocator.CreateAsyncScope();
                    var rabbitMQOptions = scope.ServiceProvider.GetRequiredService<IOptions<RabbitMQOptions>>().Value;
                    await channel.BasicConsumeAsync(rabbitMQOptions.AuthorizationServerUserCreatedQueue, autoAck: false, consumer, stoppingToken);

                    await Task.Delay(Timeout.Infinite, stoppingToken);
                }
                catch
                {
                    await Task.Delay(5000, stoppingToken);
                }
            }
        }

    }
}
