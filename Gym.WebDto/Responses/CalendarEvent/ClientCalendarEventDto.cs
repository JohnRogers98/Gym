using Gym.WebDto.Responses.Instructor;
using Gym.WebDto.Responses.Training;

namespace Gym.WebDto.Responses.CalendarEvent
{
    public record ClientCalendarEventDto
    {
        public required String Id { get; init; }
        public DateTime Start { get; init; }
        public DateTime? End { get; init; }
        public required TrainingDto Training { get; init; }
        public Int32? MaxClientCount { get; init; }
        public Int32 CurrentClientCount { get; init; }
        public IEnumerable<InstructorDto>? Instructors { get; init; }
        public Boolean IsAlreadyBooked { get; init; }
    }
}
