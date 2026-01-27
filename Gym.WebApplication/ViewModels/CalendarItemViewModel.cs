namespace Gym.WebApplication.ViewModels
{
    public record CalendarItemViewModel
    {
        public required String Id { get; init; }
        public DateTime Start { get; init; }
        public DateTime? End { get; init; }
        public required TrainingViewModel Training { get; init; }
        public Int32? MaxClientCount { get; init; }
        public IEnumerable<InstructorViewModel>? Instructors { get; init; }
    }
}
