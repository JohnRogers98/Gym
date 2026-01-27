namespace Gym.WebDto.Dto
{
    public record AdminCalendarEventDto
    {
        public required String Id { get; init; }
        public DateTime Start { get; init; }
        public DateTime? End { get; init; }
        public required TrainingDto Training { get; init; }
        public Int32? MaxClientCount { get; init; }
        public IEnumerable<InstructorDto>? Instructors { get; init; }
        public IEnumerable<String>? BookingUsers { get; init; }
    }
}
