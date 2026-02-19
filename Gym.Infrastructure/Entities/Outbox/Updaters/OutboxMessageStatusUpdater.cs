using Gym.Infrastructure.Entities.Outbox;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gym.Infrastructure.Entities.Outbox.Updaters
{
    internal interface IOutboxMessageStatusUpdater
    {
        Task UpdateMessageStatus(ObjectId messageId, ProcessingStatus status, CancellationToken cancellationToken);
    }

    internal class OutboxMessageStatusUpdater(IMongoCollection<MessageEntity> _messageCollection, MongoUnitOfWork _mongoUnitOfWork) : IOutboxMessageStatusUpdater
    {
        public async Task UpdateMessageStatus(ObjectId messageId, ProcessingStatus status, CancellationToken cancellationToken)
        {
            await _messageCollection.UpdateOneAsync(
                _mongoUnitOfWork.Session,
                entity => entity.Id == messageId,
                Builders<MessageEntity>.Update
                    .Set(entity => entity.ProcessedAt, DateTime.UtcNow)
                    .Set(entity => entity.Status, status.ToString()),
                cancellationToken: cancellationToken);
        }
    }
}
