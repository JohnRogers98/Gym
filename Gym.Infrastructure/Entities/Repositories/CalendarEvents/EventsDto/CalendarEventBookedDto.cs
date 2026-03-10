namespace Gym.Infrastructure.Entities.Repositories.CalendarEvents.EventsDto
{
    internal record CalendarEventBookedDto(String Id, DateTime occurredOn, String CalendarEventId, String UserId);
}
