using Gym.WebDto.Dto;

namespace Gym.WebDto.Responses.CalendarEvent
{
    public record GetCalendarEventResponse(
        String id,
        DateTime start,
        DateTime? end,
        TrainingDto training,
        Int32? maxClientCount,
        IEnumerable<InstructorDto>? instructors);
}
