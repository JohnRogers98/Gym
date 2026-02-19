namespace Gym.Domain.UserContext
{
    public interface IUserByTelegramIdFinder
    {
        Task<User?> GetByTelegramIdAsync(TelegramId telegramId, CancellationToken cancellationToken);

        Task<Boolean> ExistsByTelegramIdAsync(TelegramId telegramId, CancellationToken cancellationToken);
    }
}
