using Gym.Infrastructure.Entities.Outbox;

namespace Gym.Infrastructure.Entities.EventStores
{
    internal class OutboxEventStoreAspect(IEventStore _decoratee, IMessagePublisher _messagePublisher) : IEventStore
    {

        public async Task<IEnumerable<EventEntity>> LoadAsync(StreamId streamId, CancellationToken cancellationToken)
        {
            return await _decoratee.LoadAsync(streamId, cancellationToken);
        }

        public async Task SaveAutoversionedAsync(StreamId streamId, IEnumerable<EventEntity> eventEntities, CancellationToken cancellationToken)
        {
            await _decoratee.SaveAutoversionedAsync(streamId, eventEntities, cancellationToken);

            foreach (var anEventEntity in eventEntities)
            {
                await _messagePublisher.PublishAsync(
                    MessageEnvelope.Create(anEventEntity.Id, streamId.Value, anEventEntity.Version),
                    cancellationToken
                    );
            }
        }

        public async Task SaveVersionedAsync(StreamId streamId, IEnumerable<EventEntity> eventEntities, Int32 expectedVersion, CancellationToken cancellationToken)
        {
            await _decoratee.SaveVersionedAsync(streamId, eventEntities, expectedVersion, cancellationToken);

            foreach (var anEventEntity in eventEntities)
            {
                await _messagePublisher.PublishAsync(
                    MessageEnvelope.Create(anEventEntity.Id, streamId.Value, anEventEntity.Version),
                    cancellationToken
                    );
            }
        }
    }
}
