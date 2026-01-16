using Gym.Domain._Common;
using MediatR;

namespace Gym.Application.Services.DomainEventPublisher
{
    internal class DomainEventPublisher(IMediator _mediator) : IDomainEventPublisher
    {
        public async Task PublishAsync(IEnumerable<DomainEvent> domainEvents, CancellationToken cancellationToken)
        {
            foreach (var domainEvent in domainEvents)
            {
                await this.PublishAsync(domainEvent, cancellationToken);
            }
        }

        /// <summary>
        /// Dynamic generic dispatch
        /// </summary>
        /// <param name="domainEvent"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task PublishAsync(DomainEvent domainEvent, CancellationToken cancellationToken)
        {
            await PublishGenericAsync((dynamic)domainEvent, cancellationToken);
        }

        public async Task PublishGenericAsync<TDomainEvent>(TDomainEvent domainEvent, CancellationToken cancellationToken) 
            where TDomainEvent : DomainEvent
        {
            var notification = new DomainEventNotification<TDomainEvent>(domainEvent);
            await _mediator.Publish(notification, cancellationToken);
        }
    }
}
