using Gym.Domain._Common;

namespace Gym.Application.Services.DomainEventPublisher
{
    internal interface IDomainEventPublisher
    {
        Task PublishAsync(IEnumerable<DomainEvent> domainEvents, CancellationToken cancellationToken);
        Task PublishAsync(DomainEvent domainEvent, CancellationToken cancellationToken);
    }
}
