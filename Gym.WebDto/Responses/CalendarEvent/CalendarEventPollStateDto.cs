namespace Gym.WebDto.Responses.CalendarEvent
{
    public record CalendarEventPollStateDto
    {
        public required String Id { get; init; }
        public required String Title { get; init; }
        public required List<ChoiceStateInfo> Choices { get; init; }
    }

    public record ChoiceStateInfo
    {
        public required Int32 Id { get; init; }
        public required String Text { get; init; }
        public Int32 VoteCount { get; init; } = 0;
    }
}
