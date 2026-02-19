namespace Gym.Infrastructure.Entities.Outbox
{
    internal class MessageEnvelope
    {
        public required String EventId { get; set; }
        public required String StreamId { get; set; }

        public Int32? Version { get; set; }

        public String? Payload { get; set; }

        private MessageEnvelope() { }

        public static MessageEnvelope Create(String eventId, String streamId, Int32 version, String? payload = default) 
        {
            return new() 
            {
                EventId = eventId,
                StreamId = streamId,
                Version = version,
                Payload = payload
            };
        }
    }

}
