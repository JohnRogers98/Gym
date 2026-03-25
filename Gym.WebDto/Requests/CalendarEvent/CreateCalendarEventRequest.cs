namespace Gym.WebDto.Requests.CalendarEvent
{
    public record CreateCalendarEventRequest
    {
        public required DateTime Start { get; init; }
        public DateTime? End { get; init; }
        public required String TrainingId { get; init; }
        public Int32? MaxClientCount { get; init; }
        public IEnumerable<String>? Instructors { get; init; }
        public CalendarEventPollDto? Poll { get; init; }
    }

    public record CalendarEventPollDto
    {
        public required String Title { get; init; }
        public required Boolean IsResponseRequired { get; init; }
        public required Boolean CanAcceptMany { get; init; }
        public required IEnumerable<String> ChoiceVariants { get; init; }
    }
}
