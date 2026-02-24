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

        private DomainEvent ToDomainEvent(CalendarEventBookedDto dto)
        {
            return CalendarEventBookedDomainEvent.Restore(
                DomainEventId.From(Guid.Parse(dto.Id)),
                dto.occurredOn,
                CalendarEventId.From(dto.CalendarEventId),
                UserId.From(dto.UserId));
        }

        private DomainEvent ToDomainEvent(CalendarEventCompletedDto dto)
        {
            return CalendarEventCompletedDomainEvent.Restore(
                DomainEventId.From(Guid.Parse(dto.Id)),
                dto.occurredOn,
                CalendarEventId.From(dto.CalendarEventId),
                dto.BookingUsers.Select(UserId.From).ToList().AsReadOnly());
        }

        private DomainEvent ToDomainEvent(CalendarEventCancelledDto dto)
        {
            return CalendarEventCancelledDomainEvent.Restore(
                DomainEventId.From(Guid.Parse(dto.Id)),
                dto.occurredOn,
                CalendarEventId.From(dto.CalendarEventId),
                dto.BookingUsers.Select(UserId.From).ToList().AsReadOnly());
        }
    }
}
