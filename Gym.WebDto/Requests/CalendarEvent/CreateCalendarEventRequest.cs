namespace Gym.WebDto.Requests.CalendarEvent
{
    public record CreateCalendarEventRequest
    {
        public required DateTime Start { get; init; }
        public DateTime? End { get; init; }
        public required String TrainingId { get; init; }
        public Int32? MaxClientCount { get; init; }
        public IEnumerable<String>? Instructors { get; init; }
    }
}
