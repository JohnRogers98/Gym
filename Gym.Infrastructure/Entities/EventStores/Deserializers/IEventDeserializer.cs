using Gym.Domain._Common;
using Gym.Infrastructure.Entities.EventStores;

namespace Gym.Infrastructure.Entities.EventStores.Deserializers
{
    internal interface IEventDeserializer
    {
        DomainEvent Deserialize(EventEntity eventEntity);
        TDomainEvent Deserialize<TDomainEvent>(EventEntity eventEntity) where TDomainEvent : DomainEvent;
    }
}
