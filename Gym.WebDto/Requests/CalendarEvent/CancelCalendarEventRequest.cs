namespace Gym.WebDto.Requests.CalendarEvent
{
    public record CancelCalendarEventRequest
    {
        public required String CalendarEventId { get; init; }
    }
}
