namespace Gym.WebApplication.Features.Account.History.ViewModels
{
    public record TrainingCancelledViewModel : AccountHistoryItemViewModel
    {
        public required String TrainingName { get; init; }

        public override String GetMessage()
            => $"Training {TrainingName} was cancelled";
    }
}
