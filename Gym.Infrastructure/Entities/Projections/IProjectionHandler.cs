using Gym.Infrastructure.Entities.EventStores;

namespace Gym.Infrastructure.Entities.Projections
{
    internal interface IProjectionHandler
    {
        Boolean CanHandle(String aggregateType, String operation);
        Task HandleAsync(EventEntity eventEntity, CancellationToken cancellationToken);
    }
}
