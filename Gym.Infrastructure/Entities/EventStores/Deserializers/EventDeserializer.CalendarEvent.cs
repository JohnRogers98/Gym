using Gym.Application.Extensions;
using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.CalendarEventContext.Events;
using Gym.Domain.CalendarEventContext.ValueObjects;
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
                CalendarEventId.From(dto.CalendarEventId).Unwrap()
            );
        }

        private DomainEvent ToDomainEvent(CalendarEventBookedDto dto)
        {
            return CalendarEventBookedDomainEvent.Restore(
                DomainEventId.From(Guid.Parse(dto.Id)),
                dto.occurredOn,
                CalendarEventId.From(dto.CalendarEventId).Unwrap(),
                UserId.From(dto.UserId).Unwrap()
            );
        }

        private DomainEvent ToDomainEvent(CalendarEventCompletedDto dto)
        {
            return CalendarEventCompletedDomainEvent.Restore(
                DomainEventId.From(Guid.Parse(dto.Id)),
                dto.occurredOn,
                CalendarEventId.From(dto.CalendarEventId).Unwrap(),
                dto.BookingUsers.Select(userId => UserId.From(userId).Unwrap()).ToList().AsReadOnly()
            );
        }

        private DomainEvent ToDomainEvent(CalendarEventCancelledDto dto)
        {
            return CalendarEventCancelledDomainEvent.Restore(
                DomainEventId.From(Guid.Parse(dto.Id)),
                dto.occurredOn,
                CalendarEventId.From(dto.CalendarEventId).Unwrap(),
                dto.BookingUsers.Select(userId => UserId.From(userId).Unwrap()).ToList().AsReadOnly()
            );
        }
    }
}
