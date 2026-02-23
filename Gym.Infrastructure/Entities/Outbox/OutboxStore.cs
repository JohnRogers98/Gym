using MongoDB.Bson;
using MongoDB.Driver;

namespace Gym.Infrastructure.Entities.Outbox
{
    internal interface IMessagePublisher
    {
        Task PublishAsync(MessageEnvelope messageEnvelope, CancellationToken cancellationToken);
    }

    internal class OutboxStore(IMongoCollection<MessageEntity> _messageCollection, MongoUnitOfWork _mongoUnitOfWork) : IMessagePublisher
    {
        public async Task PublishAsync(MessageEnvelope messageEnvelope, CancellationToken cancellationToken)
        {
            MessageEntity? messageEntity = this.CreateMessageFromEnvelope(messageEnvelope);
            if (messageEntity == null)
                throw new ArgumentException("Incorrect data in envelope.");

            await _messageCollection.InsertOneAsync(_mongoUnitOfWork.Session, messageEntity, cancellationToken: cancellationToken);
        }

        private MessageEntity? CreateMessageFromEnvelope(MessageEnvelope messageEnvelope)
        {
            return new MessageEntity()
            {
                Id = ObjectId.GenerateNewId(),
                EventId = messageEnvelope.EventId,
                StreamId = messageEnvelope.StreamId,
                Payload = messageEnvelope.Payload,
                Version = messageEnvelope.Version,
                Status = ProcessingStatus.Created.ToString(),
                CreatedAt = DateTime.UtcNow,
            };
        }

    }
}
