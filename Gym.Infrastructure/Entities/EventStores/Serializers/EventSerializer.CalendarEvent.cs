using Gym.Domain.CalendarEventContext.Events;
using Gym.Infrastructure.Entities.Repositories.CalendarEvents.EventsDto;

namespace Gym.Infrastructure.Entities.EventStores.Serializers
{
    internal partial class EventSerializer
    {
        private CalendarEventCreatedDto ToDto(CalendarEventCreatedDomainEvent domainEvent)
        {
            return new CalendarEventCreatedDto(
                domainEvent.Id.Value.ToString(),
                domainEvent.OccurredOn,
                domainEvent.CalendarEventId.Value);
        }

        private CalendarEventBookedDto ToDto(CalendarEventBookedDomainEvent domainEvent)
        {
            return new CalendarEventBookedDto(
                domainEvent.Id.Value.ToString(),
                domainEvent.OccurredOn,
                domainEvent.CalendarEventId.Value,
                domainEvent.UserId.Value);
        }

        private CalendarEventCompletedDto ToDto(CalendarEventCompletedDomainEvent domainEvent)
        {
            return new CalendarEventCompletedDto(
                domainEvent.Id.Value.ToString(),
                domainEvent.OccurredOn,
                domainEvent.CalendarEventId.Value,
                domainEvent.BookingUsers.Select(userId => userId.Value).ToList().AsReadOnly());
        }

        private CalendarEventCancelledDto ToDto(CalendarEventCancelledDomainEvent domainEvent)
        {
            return new CalendarEventCancelledDto(
                domainEvent.Id.Value.ToString(),
                domainEvent.OccurredOn,
                domainEvent.CalendarEventId.Value,
                domainEvent.BookingUsers.Select(userId => userId.Value).ToList().AsReadOnly());
        }
    }
}
