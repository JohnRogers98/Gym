using Gym.WebDto.Dto;

namespace Gym.WebDto.Requests.CalendarEvent
{
    public record CreateCalendarEventRequest
    {
        public DateTime Start { get; init; }
        public DateTime? End { get; init; }
        public required TrainingDto Training { get; init; }
        public Int32? MaxClientCount { get; init; }
        public IEnumerable<InstructorDto>? Instructors { get; init; }
    }
}
