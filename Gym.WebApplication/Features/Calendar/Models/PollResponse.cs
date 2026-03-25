namespace Gym.WebApplication.Features.Calendar.Models
{
    public record PollResponse
    {
        public required String PollId {  get; init; }
        public required IReadOnlyCollection<Int32> SelectedChoices {  get; init; }
    }
}
