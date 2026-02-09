using Gym.Domain._Common;

namespace Gym.Infrastructure.EventStores
{
    internal abstract class EventMapper
    {
        public abstract String Serialize(DomainEvent domainEvent);
        public abstract DomainEvent Deserialize(EventEntity eventEntity);
    }
}
