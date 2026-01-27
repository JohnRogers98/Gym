namespace Gym.WebDto.Requests.Bookings
{
    public record BookTrainingEventRequest
    {
        public required String CalendarEventId { get; init; }
    }
}
