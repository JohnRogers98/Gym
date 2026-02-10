using Gym.Infrastructure.Entities;
using Gym.Infrastructure.EventStores.Extensions;
using MongoDB.Driver;

namespace Gym.Infrastructure.EventStores
{
    internal interface IEventStore
    {
        public Task SaveAsync(StreamId streamId, IEnumerable<EventEntity> eventEntities, CancellationToken cancellationToken);
        Task<IEnumerable<EventEntity>> LoadAsync(StreamId streamId, CancellationToken cancellationToken);
    }

    internal class EventStore(IMongoCollection<EventEntity> _events, MongoUnitOfWork _mongoUnitOfWork) : IEventStore
    {

        public async Task<IEnumerable<EventEntity>> LoadAsync(StreamId streamId, CancellationToken cancellationToken)
        {
            var sort = Builders<EventEntity>.Sort.Ascending(e => e.Version);

            return await _events.Find(_mongoUnitOfWork.Session, eventEntity => eventEntity.StreamId == streamId.Value)
                .Sort(sort)
                .ToListAsync(cancellationToken);
        }

        public async Task SaveAsync(StreamId streamId, IEnumerable<EventEntity> eventEntities, CancellationToken cancellationToken)
        {
            if (!eventEntities.Any())
                return;

            Int32? lastVersion = await this.GetLastVersionAsync(streamId, cancellationToken);
            if (!lastVersion.HasValue) 
            {
                lastVersion = 0;
            }

            if (eventEntities.IsVersionSequenceCorrect(lastKnownVersion: lastVersion.Value) is false)
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

            EventEntity lastEventEntity =  await _events.Find(_mongoUnitOfWork.Session, @event => @event.StreamId == streamId.Value)
                .Sort(sort)
                .FirstOrDefaultAsync(cancellationToken);

            return lastEventEntity?.Version ?? null;
        }
    }
}
