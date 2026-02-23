using Gym.Abstractions.Query.EventStore;
using MongoDB.Driver;

namespace Gym.Infrastructure.Entities.Projections.Events
{
    internal class EventProjectionStore(IMongoCollection<EventProjection> _eventProjectionCollection, MongoUnitOfWork _mongoUnitOfWork)
    {
        public async Task CreateAsync(EventProjection eventProjection, CancellationToken cancellationToken)
        {
            await _eventProjectionCollection.InsertOneAsync(_mongoUnitOfWork.Session, eventProjection, cancellationToken: cancellationToken);
        }
    }
}
