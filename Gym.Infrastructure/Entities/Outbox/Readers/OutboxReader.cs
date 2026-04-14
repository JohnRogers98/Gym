using MongoDB.Driver;

namespace Gym.Infrastructure.Entities.Outbox.Readers
{
    internal interface IOutboxReader
    {
        Task<IEnumerable<MessageEntity>> GetStalledMessagesAsync(CancellationToken cancellationToken);
        Task<IEnumerable<MessageEntity>> GetFailedMessagesAsync(CancellationToken cancellationToken);
    }

    internal class OutboxReader(IMongoCollection<MessageEntity> _messageCollection) : IOutboxReader
    {
        public async Task<IEnumerable<MessageEntity>> GetFailedMessagesAsync(CancellationToken cancellationToken)
        {
            return await _messageCollection
             .Find(entity => entity.Status == nameof(ProcessingStatus.Failed))
             .SortBy(x => x.CreatedAt)
             .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<MessageEntity>> GetStalledMessagesAsync(CancellationToken cancellationToken)
        {
            return await _messageCollection
               .Find(entity => entity.Status == nameof(ProcessingStatus.Created) || entity.Status == nameof(ProcessingStatus.PendingRecovery))
               .SortBy(x => x.CreatedAt)
               .ToListAsync(cancellationToken);
        }
    }
}
