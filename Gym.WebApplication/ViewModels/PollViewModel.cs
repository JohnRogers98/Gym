namespace Gym.WebApplication.ViewModels
{
    public record PollViewModel
    {
        public required String Id { get; init; }
        public required String Title { get; init; }
        public required List<ChoiceViewModel> Choices { get; init; }
    }

    public record ChoiceViewModel
    {
        public required Int32 Id { get; init; }
        public required String Text { get; init; }
        public Int32 VoteCount { get; init; } = 0;
    }

}
