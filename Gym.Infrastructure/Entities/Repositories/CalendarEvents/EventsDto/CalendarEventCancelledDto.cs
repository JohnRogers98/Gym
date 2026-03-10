namespace Gym.Infrastructure.Entities.Repositories.CalendarEvents.EventsDto
{
    internal record CalendarEventCancelledDto(String Id, DateTime occurredOn, String CalendarEventId, IReadOnlyCollection<String> BookingUsers);
}
