using Gym.Domain.CalendarEventContext.Events;
using Gym.Infrastructure.Scanners;

namespace Gym.Infrastructure.Entities.Repositories.CalendarEvents.EventsDto
{
    [EventSerializationForm<CalendarEventCreatedDomainEvent>]
    internal record CalendarEventCreatedDto(String Id, DateTime occurredOn, String CalendarEventId);
}
