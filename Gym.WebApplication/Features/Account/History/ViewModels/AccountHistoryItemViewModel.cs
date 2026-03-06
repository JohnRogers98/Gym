namespace Gym.WebApplication.Features.Account.History.ViewModels
{
    public abstract record AccountHistoryItemViewModel
    {
        public required DateTime OccurredAt { get; init; }

        public abstract String GetMessage();
    }
}
