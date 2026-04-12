using Gym.Domain.TelegramAuthContext;
using Gym.Domain.TelegramAuthContext.ValueObjects;

namespace Gym.Domain.TelegramAuthContext
{
    public interface ITelegramAuthRepository
    {
        Task SaveAsync(TelegramAuth telegramAuth, CancellationToken cancellationToken);
        Task<TelegramAuth?> GetByIdAsync(TelegramId id, CancellationToken cancellationToken);
        Task<Boolean> ExistsAsync(TelegramId id, CancellationToken cancellationToken);
    }
}
