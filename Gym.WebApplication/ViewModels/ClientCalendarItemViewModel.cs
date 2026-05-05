using Gym.WebApplication.Features._Common;

namespace Gym.WebApplication.ViewModels
{
    public record ClientCalendarItemViewModel : ITimeBasedItem
    {
        public required String Id { get; init; }

        public DateTime UtcStart { get; init; }

        public DateTime? UtcEnd { get; init; }

        public required String Status { get; set; }
        public required TrainingViewModel Training { get; init; }
        public Int32? MaxClientCount { get; init; }
        public Int32 CurrentClientCount { get; init; }
        public IEnumerable<InstructorViewModel>? Instructors { get; init; }
        public Boolean IsAlreadyBooked { get; init; }

        public Boolean IsFull() => MaxClientCount.HasValue && MaxClientCount.Value == CurrentClientCount;

        public Boolean IsUpcoming => Status == "Upcoming";

        public PollViewModel? Poll { get; init; }

        public DateTime Start => UtcStart.ToLocalTime();

        public DateTime? End => UtcEnd?.ToLocalTime();
    }
}
