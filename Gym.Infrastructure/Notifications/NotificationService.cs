using Gym.Domain._Shared;
using Gym.Domain.UserContext;

namespace Gym.Infrastructure.Notifications
{
    internal class NotificationService : INotificationService
    {
        private readonly IEnumerable<INotificationChannel> _notificationChannels;
        
        public NotificationService(IEnumerable<INotificationChannel> notificationChannels)
        {
            _notificationChannels = notificationChannels;
        }

        public async Task SendMessageAsync(UserId userId, String message, CancellationToken cancellationToken)
        {
            foreach (var aChannel in _notificationChannels)
            {
                try
                {
                    await aChannel.SendMessageAsync(userId, message, cancellationToken);
                }
                catch 
                {
                    //TODO: Log error
                }
            }
        }
    }
}
