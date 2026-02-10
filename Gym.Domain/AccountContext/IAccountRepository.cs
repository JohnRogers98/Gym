namespace Gym.Domain.AccountContext
{
    public interface IAccountRepository
    {
        Task SaveAsync(Account account, CancellationToken cancellationToken);
        Task<Account> GetByIdAsync(AccountId accountId, CancellationToken cancellationToken);
    }
}
