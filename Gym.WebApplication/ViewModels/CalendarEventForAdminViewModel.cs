using Gym.WebApplication.Features._Common;

namespace Gym.WebApplication.ViewModels
{
    public record CalendarEventForAdminViewModel : ITimeBasedItem
    {
        public required String Id { get; init; }

        public DateTime UtcStart { get; init; }

        public DateTime? UtcEnd { get; init; }

        public required String Status { get; set; }
        public required TrainingViewModel Training { get; init; }
        public Int32? MaxClientCount { get; init; }
        public IEnumerable<InstructorViewModel>? Instructors { get; init; }
        public IEnumerable<BookingUserViewModel>? BookingUsers { get; init; }

        public String InstructorNames => Instructors is not null && Instructors.Any()
            ? String.Join(", ", Instructors.Select(i => i.FullName))
            : String.Empty;

        public Boolean IsUpcoming => Status == "Upcoming";

        public DateTime Start => UtcStart.ToLocalTime();

        public DateTime? End => UtcEnd?.ToLocalTime();
    }
}
