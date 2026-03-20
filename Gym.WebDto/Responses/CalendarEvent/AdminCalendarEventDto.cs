using Gym.WebDto.Responses.Bookings;
using Gym.WebDto.Responses.Instructor;
using Gym.WebDto.Responses.Training;

namespace Gym.WebDto.Responses.CalendarEvent
{
    public record AdminCalendarEventDto
    {
        public required String Id { get; init; }
        public DateTime Start { get; init; }
        public DateTime? End { get; init; }
        public required String Status { get; init; }
        public required TrainingInfo Training { get; init; }
        public Int32? MaxClientCount { get; init; }
        public IEnumerable<InstructorInfo>? Instructors { get; init; }
        public IEnumerable<BookingUserInfo>? BookingUsers { get; init; }

        public CalendarEventPollStateDto? PollInfo { get; init; }
    }
}
