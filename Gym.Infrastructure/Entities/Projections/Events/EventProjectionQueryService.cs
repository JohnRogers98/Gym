using Gym.Abstractions.Query.EventStore;
using MongoDB.Driver;

namespace Gym.Infrastructure.Entities.Projections.Events
{
    internal class EventProjectionQueryService(IMongoCollection<EventProjection> _eventProjectionCollection) : IEventProjectionQueryService
    {
        public async Task<IEnumerable<EventProjection>> GetByStreamId(String streamId, CancellationToken cancellationToken)
        {
            return await _eventProjectionCollection.Find(eventProjection => eventProjection.StreamId == streamId)
                .SortBy(eventEntity => eventEntity.Version)
                .ToListAsync(cancellationToken);
        }
    }
}
