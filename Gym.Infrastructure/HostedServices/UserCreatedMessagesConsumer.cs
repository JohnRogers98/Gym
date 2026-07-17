using Gym.Application.Services.UserApi.CreateClient;
using Gym.Domain._Common;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
                                await channel.BasicNackAsync(ea.DeliveryTag, false, true, stoppingToken);
                                return;
                            }

                            CreateUser createUser = new(message.UserId, message.Role, message.FirstName!, message.LastName);
                            Result createdUserResult = await mediator.Send(createUser);

                            if (createdUserResult.Success)
                            {
                                await channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
                            }
                            else
                            {
                                await channel.BasicNackAsync(ea.DeliveryTag, false, false, stoppingToken);
                            }
                        }
                        catch
                        {
                            await channel.BasicNackAsync(ea.DeliveryTag, false, false, stoppingToken);
                        }
                    };

                    await channel.BasicConsumeAsync("created-users-worker-queue", autoAck: false, consumer, stoppingToken);

                    await Task.Delay(Timeout.Infinite, stoppingToken);
                }
                catch
                {
                    await Task.Delay(5000, stoppingToken);
                }
            }
        }

    }

    public record UserCreatedMessage(String UserId, String? FirstName, String? LastName, String Role);
}
