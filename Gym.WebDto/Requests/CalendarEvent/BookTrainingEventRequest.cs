namespace Gym.WebDto.Requests.CalendarEvent
{
    public record BookTrainingEventRequest
    {
        public required String CalendarEventId { get; init; }
        public CalendarEventPollResponseDto? PollResponse { get; init; }
    }

    public record CalendarEventPollResponseDto
    {
        public required String PollId { get; init; }
        public required IEnumerable<Int32> SelectedChoices { get; init; }
    }
}
