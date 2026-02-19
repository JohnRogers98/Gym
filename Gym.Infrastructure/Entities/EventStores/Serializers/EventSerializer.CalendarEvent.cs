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
    }
}
