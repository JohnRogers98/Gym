using Gym.Infrastructure.Entities.EventStores;
using MongoDB.Driver;

namespace Gym.Infrastructure.Entities.EventStores.Readers
{
    internal interface IEventStoreReader
    {
        Task<EventEntity?> GetByIdAsync(String eventId, CancellationToken cancellationToken);
    }

    internal class EventStoreReader(IMongoCollection<EventEntity> _events) : IEventStoreReader
    {
        public async Task<EventEntity?> GetByIdAsync(String eventId, CancellationToken cancellationToken)
        {
            return await _events.Find(eventEntity => eventEntity.Id == eventId)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
