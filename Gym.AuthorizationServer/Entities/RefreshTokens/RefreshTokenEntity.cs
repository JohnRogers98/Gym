using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gym.AuthorizationServer.Entities.RefreshTokens
{
    public class RefreshTokenEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; } = default!;

        public required String Token { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public required String AccessTokenId { get; set; }

        public required DateTime ExpiresAt { get; set; }

        public String? Acr { get; set; }
        public List<String>? Amr { get; set; }
    }
}
