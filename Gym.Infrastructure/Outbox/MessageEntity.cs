using MongoDB.Bson;

namespace Gym.Infrastructure.Outbox
{
    internal class MessageEntity
    {
        public required ObjectId Id { get; set; }

        public required String SourceDiscriminator { get; set; }

        public String? EventId { get; set; }
        public String? StreamId { get; set; }

        public String? EntityId { get; set; }
        public String? EntityType { get; set; }

        public Int32? Version { get; set; }

        public String? OperationType { get; set; } 

        public String? Payload { get; set; }

        public required String Status { get; set; }

        public required DateTime CreatedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
    }
}
