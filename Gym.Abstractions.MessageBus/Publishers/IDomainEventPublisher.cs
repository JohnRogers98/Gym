using Gym.Domain._Common;

namespace Gym.Abstractions.MessageBus.Publishers
{
    public interface IDomainEventPublisher
    {
        Task PublishAsync(IEnumerable<DomainEvent> domainEvents, CancellationToken cancellationToken);
        Task PublishAsync(DomainEvent domainEvent, CancellationToken cancellationToken);
    }
}
