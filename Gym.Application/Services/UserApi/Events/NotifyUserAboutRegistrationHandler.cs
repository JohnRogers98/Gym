using Gym.Application.Services.DomainEventPublisher;
using Gym.Domain.UserAggregate;
using Gym.Domain.UserAggregate.Events;
using MediatR;

namespace Gym.Application.Services.UserApi.Events
{
    internal class NotifyUserAboutRegistrationHandler(INotificationService _notificationService) 
        : INotificationHandler<DomainEventNotification<CreatedNewClientDomainEvent>>
    {
        public async Task Handle(DomainEventNotification<CreatedNewClientDomainEvent> notification, CancellationToken cancellationToken)
        {
            await _notificationService.SendMessageAsync(
                notification.DomainEvent.UserId,
                $"Welcome to Gym! Your internal id - {notification.DomainEvent.UserId.Value}",
                cancellationToken);
        }
    }
}
