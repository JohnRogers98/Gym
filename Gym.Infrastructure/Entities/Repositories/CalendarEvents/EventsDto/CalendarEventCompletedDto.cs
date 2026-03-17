using Gym.Domain.CalendarEventContext.Events;
using Gym.Infrastructure.Scanners;

namespace Gym.Infrastructure.Entities.Repositories.CalendarEvents.EventsDto
{
    [EventSerializationForm<CalendarEventCompletedDomainEvent>]
    internal record CalendarEventCompletedDto(String Id, DateTime occurredOn, String CalendarEventId, IReadOnlyCollection<String> BookingUsers);
}
