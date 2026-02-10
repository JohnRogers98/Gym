using Gym.Domain.AccountContext;

namespace Gym.Application.Services.AccountApi
{
    internal static class AccountExtensions
    {
        public static AccountDetails ToDetails(this Account account) => new AccountDetails();
    }
}
