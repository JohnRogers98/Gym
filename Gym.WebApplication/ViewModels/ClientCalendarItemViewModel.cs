namespace Gym.WebApplication.ViewModels
{
    public record ClientCalendarItemViewModel
    {
        public required String Id { get; init; }
        public DateTime Start { get; init; }
        public DateTime? End { get; init; }
        public required String Status { get; set; }
        public required TrainingViewModel Training { get; init; }
        public Int32? MaxClientCount { get; init; }
        public Int32 CurrentClientCount { get; init; }
        public IEnumerable<InstructorViewModel>? Instructors { get; init; }
        public Boolean IsAlreadyBooked { get; init; }

        public Boolean IsFull() => MaxClientCount.HasValue && MaxClientCount.Value == CurrentClientCount;

        public Boolean IsUpcoming => Status == "Upcoming";

        public DateTime LocalStart => Start.ToLocalTime();
        public DateTime? LocalEnd => End?.ToLocalTime();
    }
}
