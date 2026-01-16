using Gym.Domain._Common;
using MediatR;

namespace Gym.Application.Services.DomainEventPublisher
{
    internal class DomainEventNotification<TDomainEvent> : INotification
        where TDomainEvent : DomainEvent
    {
        public TDomainEvent DomainEvent { get; }
        public DomainEventNotification(TDomainEvent domainEvent) => DomainEvent = domainEvent;
    }
}
