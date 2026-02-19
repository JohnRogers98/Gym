using Gym.Abstractions.MessageBus.Publishers;
using Gym.Domain.AccountContext.Events;
using Gym.Domain.UserContext;
using MediatR;

namespace Gym.Abstractions.MessageBus.Notifiers.Accounts
{
    internal class NotifyUserAboutAccountCharging(INotificationService _notificationService)
         : INotificationHandler<DomainEventNotification<AccountChargedDomainEvent>>
    {
        public async Task Handle(DomainEventNotification<AccountChargedDomainEvent> notification, CancellationToken cancellationToken)
        {
            await _notificationService.SendMessageAsync(
              notification.DomainEvent.UserId,
              $"Your account charged by {notification.DomainEvent.ByCount}",
              cancellationToken);
        }
    }
}
