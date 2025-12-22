using Gym.WebDto.Dto;

namespace Gym.WebDto.Responses.CalendarEvent
{
    public record CreateCallendarEventResponse(
        String id,
        DateTime start,
        DateTime? end,
        TrainingDto training,
        Int32? maxClientCount,
        IEnumerable<InstructorDto>? instructors);
}
