using Gym.Domain.CalendarEventContext.Events;
using Gym.Infrastructure.Scanners;

namespace Gym.Infrastructure.Entities.Repositories.CalendarEvents.EventsDto
{
    [EventSerializationForm<CalendarEventBookedDomainEvent>]
    internal record CalendarEventBookedDto(String Id, DateTime occurredOn, String CalendarEventId, String UserId);
}
