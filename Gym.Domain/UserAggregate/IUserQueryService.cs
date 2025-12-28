namespace Gym.Domain.UserAggregate
{
    public interface IUserQueryService
    {
        Task<User?> GetByTelegramIdAsync(TelegramId telegramUserId, CancellationToken cancellationToken);

        Task<Boolean> ExistsByTelegramIdAsync(TelegramId telegramUserId, CancellationToken cancellationToken);

        Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken);
    }
}
