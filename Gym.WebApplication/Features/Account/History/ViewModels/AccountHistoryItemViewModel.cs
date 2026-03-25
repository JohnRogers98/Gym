namespace Gym.WebApplication.Features.Account.History.ViewModels
{
    public abstract record AccountHistoryItemViewModel
    {
        public required DateTime UtcOccurredAt { get; init; }
        public DateTime LocalOccurredAt => UtcOccurredAt.ToLocalTime();

        public abstract String GetMessage();
    }
}
