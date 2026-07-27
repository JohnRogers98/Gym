using Gym.RabbitMQ.Topology;
using Microsoft.Extensions.Hosting;

namespace Gym.Infrastructure.HostedServices
{
    public class MessageBusInitializer(IRabbitMQTopologyInitializer _rabbitMQTopologyInitializer) : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _rabbitMQTopologyInitializer.DeclareUserEventMessagesDeadLetterExchangeAsync(cancellationToken);
            await _rabbitMQTopologyInitializer.DeclareUserEventMessagesDeadLetterQueueAsync(cancellationToken);
            await _rabbitMQTopologyInitializer.DeclareUserEventMessagesExchangeAsync(cancellationToken);
            await _rabbitMQTopologyInitializer.DeclareUserCreatedQueueAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
