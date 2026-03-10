namespace Gym.WebApplication.Features.Account.History.ViewModels
{
    public record AccountChargedViewModel : AccountHistoryItemViewModel
    {
        public required Int32 ByCount { get; init; }
        public String? Reason { get; init; }

        public override String GetMessage()
            => $"Your account was charged by {ByCount} trainings. {Reason ?? String.Empty}";
    }
}
