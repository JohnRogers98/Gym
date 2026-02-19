using Gym.Infrastructure.Entities.EventStores;

namespace Gym.Infrastructure.Entities.Projections
{
    internal class CompositeProjectionHandler(IEnumerable<IProjectionHandler> _projectionFactories) : IProjectionHandler
    {
        public Boolean CanHandle(String aggregateType, String operation)
        {
            return _projectionFactories.Any(aFactory => aFactory.CanHandle(aggregateType, operation));
        }

        public async Task HandleAsync(EventEntity eventEntity, CancellationToken cancellationToken)
        {
            var foundedFactories = _projectionFactories.Where(aFactory => aFactory.CanHandle(eventEntity.AggregateType, eventEntity.Operation));
            
            foreach(var aFactory in foundedFactories)
            {
                await aFactory.HandleAsync(eventEntity, cancellationToken);
            }
        }
    }
}
