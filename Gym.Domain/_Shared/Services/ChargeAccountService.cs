using Gym.Domain._Common;
using Gym.Domain.AccountContext;

namespace Gym.Domain._Shared.Services
{
    public interface IChargeAccountService
    {
        Result ChargeAccount(Account account, Int32 byCount);
    }

    public class ChargeAccountService : IChargeAccountService
    {
        public Result ChargeAccount(Account account, Int32 byCount)
        {
            return account.Charge(byCount);
        }
    }
}
