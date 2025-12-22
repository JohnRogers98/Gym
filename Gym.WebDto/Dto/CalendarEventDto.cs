namespace Gym.WebDto.Dto
{
    public record CalendarEventDto(
        String id,
        DateTime start,
        DateTime? end,
        TrainingDto training,
        Int32? maxClientCount,
        IEnumerable<InstructorDto>? instructors);
}
