using Gym.WebDto.Responses.Bookings;
using Gym.WebDto.Responses.Instructor;
using Gym.WebDto.Responses.Training;

namespace Gym.WebApplication.ViewModels
{
    public record AdminCalendarItemViewModel
    {
        public required String Id { get; init; }
        public DateTime Start { get; init; }
        public DateTime? End { get; init; }
        public required String Status { get; set; }
        public required TrainingInfo Training { get; init; }
        public Int32? MaxClientCount { get; init; }
        public IEnumerable<InstructorInfo>? Instructors { get; init; }
        public IEnumerable<BookingUserInfo>? BookingUsers { get; init; }

        public String InstructorNames => Instructors is not null && Instructors.Any()
            ? String.Join(", ", Instructors.Select(i => i.FullName))
            : String.Empty;

        public Boolean IsUpcoming => Status == "Upcoming";

        public DateTime LocalStart => Start.ToLocalTime();
        public DateTime? LocalEnd => End?.ToLocalTime();
    }
}
