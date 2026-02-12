namespace Gym.Infrastructure.Outbox
{
    internal class MessageEnvelope
    {
        public String? EventId { get; set; }
        public String? StreamId { get; set; }

        public String? EntityId { get; set; }
        public String? EntityType { get; set; }

        public Int32? Version { get; set; }

        public String? Payload { get; set; }

        public SourceDiscriminator SourceDiscriminator { get; private set; } = default!;

        private MessageEnvelope() { }

        public static MessageEnvelope CreateForEvent(String eventId, String streamId, Int32 version) 
        {
            return new() 
            {
                SourceDiscriminator = SourceDiscriminator.EventSourcing,
                EventId = eventId,
                StreamId = streamId,
                Version = version
            };
        }

        public static MessageEnvelope CreateForAggregate(String entityId, String entityType, String paylaod, Int32? version)
        {
            return new()
            {
                SourceDiscriminator = SourceDiscriminator.Aggregate,
                EntityId = entityId,
                EntityType = entityType,
                Payload = paylaod,
                Version = version
            };
        }
    }

}
