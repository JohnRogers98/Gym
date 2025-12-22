namespace Gym.WebApplication.ViewModels
{
    public record CalendarItemViewModel(
        String id,
        DateTime start,
        DateTime? end,
        TrainingViewModel training,
        Int32? maxClientCount,
        IEnumerable<InstructorViewModel>? instructors);
}
