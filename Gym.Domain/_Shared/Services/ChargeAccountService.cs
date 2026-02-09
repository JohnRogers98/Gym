using Gym.Domain.AccountContext;

namespace Gym.Domain._Shared.Services
{
    public interface IChargeAccountService
    {
        void ChargeAccount(Account account, Int32 byCount);
    }

    public class ChargeAccountService : IChargeAccountService
    {
        public void ChargeAccount(Account account, Int32 byCount)
        {
            account.Charge(byCount);
        }
    }
}
