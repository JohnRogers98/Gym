using Gym.WebDto.Responses.Bookings;
using Gym.WebDto.Responses.Instructor;
using Gym.WebDto.Responses.Training;

namespace Gym.WebApplication.ViewModels
{
    public record CalendarEventForAdminViewModel
    {
        public required String Id { get; init; }

        public DateTime UtcStart { get; init; }
        public DateTime LocalStart => UtcStart.ToLocalTime();

        public DateTime? UtcEnd { get; init; }
        public DateTime? LocalEnd => UtcEnd?.ToLocalTime();

        public required String Status { get; set; }
        public required TrainingInfo Training { get; init; }
        public Int32? MaxClientCount { get; init; }
        public IEnumerable<InstructorInfo>? Instructors { get; init; }
        public IEnumerable<BookingUserInfo>? BookingUsers { get; init; }

        public String InstructorNames => Instructors is not null && Instructors.Any()
            ? String.Join(", ", Instructors.Select(i => i.FullName))
            : String.Empty;

        public Boolean IsUpcoming => Status == "Upcoming";

    }
}
