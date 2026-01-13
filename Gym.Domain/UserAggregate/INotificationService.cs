namespace Gym.Domain.UserAggregate
{
    public interface INotificationService
    {
        Task SendMessageAsync(UserId userId, String message);
    }
}
