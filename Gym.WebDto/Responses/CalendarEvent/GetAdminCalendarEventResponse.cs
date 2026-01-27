using Gym.WebDto.Dto;

namespace Gym.WebDto.Responses.CalendarEvent
{
    public record GetAdminCalendarEventResponse
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
