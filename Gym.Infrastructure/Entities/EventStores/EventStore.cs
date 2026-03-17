using Gym.Infrastructure.Entities.Extensions;
using MongoDB.Driver;

namespace Gym.Infrastructure.Entities.EventStores
{
    internal interface IEventStore
    {
        Task SaveVersionedAsync(StreamId streamId, IEnumerable<EventEntity> eventEntities, Int32 expectedVersion, CancellationToken cancellationToken);
        Task SaveAutoversionedAsync(StreamId streamId, IEnumerable<EventEntity> eventEntities, CancellationToken cancellationToken);
        
        Task<IEnumerable<EventEntity>> LoadAsync(StreamId streamId, CancellationToken cancellationToken);
    }

    internal class EventStore(IMongoCollection<EventEntity> _events, MongoUnitOfWork _mongoUnitOfWork) : IEventStore
    {

        public async Task<IEnumerable<EventEntity>> LoadAsync(StreamId streamId, CancellationToken cancellationToken)
        {
            return await _events.Find(_mongoUnitOfWork.Session, eventEntity => eventEntity.StreamId == streamId.Value)
                .SortBy(eventEntity => eventEntity.Version)
                .ToListAsync(cancellationToken);
        }

        public async Task SaveAutoversionedAsync(StreamId streamId, IEnumerable<EventEntity> eventEntities, CancellationToken cancellationToken)
        {
            if (!eventEntities.Any())
                return;

            Int32 lastKnownVersion = await this.GetLastVersionAsync(streamId, cancellationToken) ?? default;

            Int32 newVersion = lastKnownVersion;
            foreach (var anEventEntity in eventEntities)
            {
                newVersion++;
                anEventEntity.Version = newVersion;
            }
            await this.SaveVersionedAsync(streamId, eventEntities, lastKnownVersion, cancellationToken);
        }

        public async Task SaveVersionedAsync(StreamId streamId, IEnumerable<EventEntity> eventEntities, Int32 lastKnownVersion, CancellationToken cancellationToken)
        {
            if (!eventEntities.Any())
                return;

            if (eventEntities.IsVersionSequenceCorrect(lastKnownVersion: lastKnownVersion) is false)
            {
                throw new ArgumentException($"Version sequence is not correct for stream - {streamId}");
            }

            await _events.InsertManyAsync(_mongoUnitOfWork.Session, eventEntities, cancellationToken: cancellationToken);
        }

        private async Task<Int32?> GetLastVersionAsync(StreamId streamId, CancellationToken cancellationToken)
        {
            var sort = Builders<EventEntity>.Sort.Descending(e => e.Version);
            var options = new FindOptions<EventEntity>
            {
                Limit = 1,
                Sort = sort
            };

            EventEntity? lastEventEntity =  await _events.Find(_mongoUnitOfWork.Session, @event => @event.StreamId == streamId.Value)
                .Sort(sort)
                .FirstOrDefaultAsync(cancellationToken);

            return lastEventEntity?.Version ?? null;
        }
    }
}
