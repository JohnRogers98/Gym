namespace Gym.WebDto.Requests.CalendarEvent
{
    public record BookCalendarEventRequest
    {
        public required String CalendarEventId { get; init; }
    }
}
