namespace Gym.WebDto.Requests.CalendarEvent
{
    public record BookTrainingEventRequest
    {
        public required String CalendarEventId { get; init; }
    }
}
