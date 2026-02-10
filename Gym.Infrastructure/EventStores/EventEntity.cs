namespace Gym.Infrastructure.EventStores
{
    internal class EventEntity
    {
        public required String Id { get; set; }

        public required String StreamId { get; set; }

        public required Int32 Version { get; set; }

        public required String Operation { get; set; }

        public required String Data { get; set; }

        public required DateTime OccurredAt { get; set; }
        public DateTime SavedAt { get; set; } = DateTime.UtcNow;
    }
}
