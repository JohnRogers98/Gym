using Gym.Abstractions.MessageBus.Publishers;
using Gym.Domain.ClientContext.Events;
using Gym.Domain.UserContext;
using MediatR;

namespace Gym.Abstractions.MessageBus.Notifiers.Users
{
    internal class NotifyUserAboutRegistrationHandler(INotificationService _notificationService) 
        : INotificationHandler<DomainEventNotification<ClientCreatedDomainEvent>>
    {
        public async Task Handle(DomainEventNotification<ClientCreatedDomainEvent> notification, CancellationToken cancellationToken)
        {
            await _notificationService.SendMessageAsync(
                notification.DomainEvent.UserId,
                $"Welcome to Gym! Your internal id - {notification.DomainEvent.Id.Value}",
                cancellationToken);
        }
    }
}
