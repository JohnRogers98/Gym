namespace Gym.Infrastructure.Entities.Repositories.CalendarEvents.EventsDto
{
    internal record CalendarEventCompletedDto(String Id, DateTime occurredOn, String CalendarEventId, IReadOnlyCollection<String> BookingUsers);
}
