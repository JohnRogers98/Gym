using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.ClientContext.Events;
using Gym.Infrastructure.Entities.Repositories.Clients.EventsDto;

namespace Gym.Infrastructure.Entities.EventStores.Deserializers
{
    internal partial class EventDeserializer
    {
        private DomainEvent ToDomainEvent(ClientCreatedDto dto)
        {
            return ClientCreatedDomainEvent.Restore(
                DomainEventId.From(Guid.Parse(dto.Id)),
                dto.occurredOn,
                UserId.From(dto.UserId));
        }
    }
}
