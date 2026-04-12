using Gym.Domain._Shared;

namespace Gym.Domain.TelegramAuthContext
{
    public interface ITelegramAuthByUserIdFinder
    {
        Task<TelegramAuth?> GetTelegramAuthByUserIdAsync(UserId userId, CancellationToken cancellationToken);
    }
}
