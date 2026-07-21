namespace Gym.WebApplication.ViewModels.AccountHistory
{
    public record AccountCreatedViewModel : AccountHistoryItemViewModel
    {
        public override String GetMessage()
            => "Account created";
    }
}
