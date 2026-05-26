using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gym.AuthorizationServer.Entities.UserConsents
{
    public class UserConsentEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; } = default!;

        [BsonRepresentation(BsonType.ObjectId)]
        public required String UserId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public required String ClientId { get; set; }

        public List<String> GrantedScopes { get; set; } = new List<String>();

        public DateTime GrantedAt { get; set; }

        public DateTime? ExpiresAt { get; set; }
    }
}
