using Gym.AuthorizationServer.Services.Events;

namespace Gym.AuthorizationServer.Integration.Tests.Fakes
{
    internal class FakeUserCreatedEventService : IUserCreatedEventService
    {
        public Task PublishAsync(UserCreatedEvent userCreatedEvent, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
