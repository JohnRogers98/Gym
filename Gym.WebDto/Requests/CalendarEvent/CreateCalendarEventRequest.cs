using Gym.WebDto.Dto;

namespace Gym.WebDto.Requests.CalendarEvent
{
    public record CreateCalendarEventRequest(
        DateTime start,
        DateTime? end,
        TrainingDto training,
        Int32? maxClientCount,
        IEnumerable<InstructorDto>? instructors);
}
