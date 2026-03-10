namespace Gym.WebApplication.Features.Account.History.ViewModels
{
    public record AccountCreatedViewModel : AccountHistoryItemViewModel
    {
        public override String GetMessage()
            => "Account created";
    }
}
