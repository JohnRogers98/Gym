using Gym.RabbitMQ.Topology.Messages;
using Gym.RabbitMQ.Topology.Services;

namespace Gym.AuthorizationServer.Integration.Tests.Fakes
{
    internal class FakeUserCreatedEventService : IUserCreatedEventService
    {
        public Task PublishAsync(UserCreatedMessage message, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
