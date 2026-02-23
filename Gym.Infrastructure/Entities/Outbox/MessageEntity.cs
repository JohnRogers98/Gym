using MongoDB.Bson;

namespace Gym.Infrastructure.Entities.Outbox
{
    internal class MessageEntity
    {
        public required ObjectId Id { get; set; }

        public required String EventId { get; set; }
        public required String StreamId { get; set; }

        public Int32? Version { get; set; }

        public String? Payload { get; set; }

        public required String Status { get; set; }

        public required DateTime CreatedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
    }
}
