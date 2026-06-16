using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gym.AuthorizationServer.Infrastructure.Entities.UserConsents
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

        [BsonRepresentation(BsonType.ObjectId)]
        public required String ProtectedResourceId { get; set; }

        public ICollection<ScopeInfo> GrantedScopes { get; set; } = new List<ScopeInfo>();

        public DateTime GrantedAt { get; set; }

        public DateTime? ExpiresAt { get; set; }
    }
}
