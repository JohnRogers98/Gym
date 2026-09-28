using Gym.Domain._Shared;

namespace Gym.Domain.UserContext
{
    public interface INotificationService
    {
        Task SendMessageAsync(UserId userId, String message, CancellationToken cancellationToken);
    }

    public interface INotificationChannel
    {
        Task SendMessageAsync(UserId userId, String message, CancellationToken cancellationToken);
    }
}
