using Gym.Domain._Common;

namespace Gym.Infrastructure.Entities.EventStores.Serializers
{
    internal interface IEventSerializer
    {
        String Serialize(DomainEvent domainEvent);
    }
}
