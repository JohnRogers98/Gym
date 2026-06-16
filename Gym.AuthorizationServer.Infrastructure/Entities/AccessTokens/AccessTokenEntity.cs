using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gym.AuthorizationServer.Infrastructure.Entities.AccessTokens
{
    public class AccessTokenEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; } = default!;

        public required String Token { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public required String UserId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public required String ClientId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public String? ProtectedResourceId { get; set; }

        public DateTime ExpiresAt { get; set; }
    }
}
