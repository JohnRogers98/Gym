namespace Gym.Abstractions.Query.EventStore
{
    public class EventProjection
    {
        public required String Id { get; set; }
        public required String StreamId { get; set; }

        public required Int32 Version { get; set; }

        public required String Operation { get; set; }

        public required Dictionary<String, Object> Payload { get; set; }

        public required DateTime OccurredAt { get; set; }
    }
}
