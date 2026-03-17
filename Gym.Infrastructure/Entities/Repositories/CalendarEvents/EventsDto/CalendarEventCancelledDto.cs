using Gym.Domain.CalendarEventContext.Events;
using Gym.Infrastructure.Scanners;

namespace Gym.Infrastructure.Entities.Repositories.CalendarEvents.EventsDto
{
    [EventSerializationForm<CalendarEventCancelledDomainEvent>]
    internal record CalendarEventCancelledDto(String Id, DateTime occurredOn, String CalendarEventId, IReadOnlyCollection<String> BookingUsers);
}
