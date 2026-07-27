namespace Gym.WebApplication.ViewModels.AccountHistory
{
    public record TrainingCompletedViewModel : AccountHistoryItemViewModel
    {
        public required String TrainingName { get; init; }

        public override String GetMessage()
            => $"Training {TrainingName} was completed";
    }
}
