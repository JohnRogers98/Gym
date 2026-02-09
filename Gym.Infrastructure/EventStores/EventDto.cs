using Gym.Domain._Common;

namespace Gym.Infrastructure.EventStores
{
    internal abstract record EventDto
    {
        public abstract DomainEvent ToDomainEvent();
    }
}
