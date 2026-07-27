using Gym.RabbitMQ.Topology;

namespace Gym.AuthorizationServer.Admin.HostedServices
{
    public class MessageBusInitializer(IRabbitMQTopologyInitializer _rabbitMQTopologyInitializer) : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _rabbitMQTopologyInitializer.DeclareUserEventMessagesDeadLetterExchangeAsync(cancellationToken);
            await _rabbitMQTopologyInitializer.DeclareUserEventMessagesDeadLetterQueueAsync(cancellationToken);
            await _rabbitMQTopologyInitializer.DeclareUserEventMessagesExchangeAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
