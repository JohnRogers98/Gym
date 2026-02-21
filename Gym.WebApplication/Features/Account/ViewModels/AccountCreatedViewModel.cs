namespace Gym.WebApplication.Features.Account.ViewModels
{
    public record AccountCreatedViewModel : AccountHistoryItemViewModel
    {
        public override String GetMessage()
            => "Account created";
    }
}
