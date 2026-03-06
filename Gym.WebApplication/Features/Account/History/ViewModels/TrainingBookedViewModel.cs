namespace Gym.WebApplication.Features.Account.History.ViewModels
{
    public record TrainingBookedViewModel : AccountHistoryItemViewModel
    {
        public required String TrainingName { get; init; }

        public override String GetMessage()
            => $"Training {TrainingName} was booked";
    }
}
