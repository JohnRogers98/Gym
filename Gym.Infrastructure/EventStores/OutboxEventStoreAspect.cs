using Gym.Infrastructure.Outbox;

namespace Gym.Infrastructure.EventStores
{
    internal class OutboxEventStoreAspect(IEventStore _decoratee, IMessagePublisher _messagePublisher) : IEventStore
    {

        public async Task<IEnumerable<EventEntity>> LoadAsync(StreamId streamId, CancellationToken cancellationToken)
        {
            return await _decoratee.LoadAsync(streamId, cancellationToken);
        }

        public async Task SaveAsync(StreamId streamId, IEnumerable<EventEntity> eventEntities, CancellationToken cancellationToken)
        {
            await _decoratee.SaveAsync(streamId, eventEntities, cancellationToken);

            foreach (var anEventEntity in eventEntities) 
            {
                await _messagePublisher.PublishAsync(
                    MessageEnvelope.CreateForEvent(anEventEntity.Id, streamId.Value, anEventEntity.Version),
                    cancellationToken
                    );
            }
        }
    }
}
