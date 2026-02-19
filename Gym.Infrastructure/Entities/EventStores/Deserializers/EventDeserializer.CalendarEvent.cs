using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.CalendarEventContext.Events;
using Gym.Infrastructure.Entities.Repositories.CalendarEvents.EventsDto;

namespace Gym.Infrastructure.Entities.EventStores.Deserializers
{
    internal partial class EventDeserializer
    {
        private DomainEvent ToDomainEvent(CalendarEventCreatedDto dto)
        {
            return CalendarEventCreatedDomainEvent.Restore(
                DomainEventId.From(Guid.Parse(dto.Id)),
                dto.occurredOn,
                CalendarEventId.From(dto.CalendarEventId));
        }
    }
}
