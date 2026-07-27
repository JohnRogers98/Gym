namespace Gym.WebApplication.ViewModels.AccountHistory
{
    public abstract record AccountHistoryItemViewModel
    {
        public required DateTime UtcOccurredAt { get; init; }
        public DateTime LocalOccurredAt => UtcOccurredAt.ToLocalTime();

        public abstract String GetMessage();
    }
}
