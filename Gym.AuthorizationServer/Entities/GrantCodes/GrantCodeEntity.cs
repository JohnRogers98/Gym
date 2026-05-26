using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gym.AuthorizationServer.Entities.GrantCodes
{
    public class GrantCodeEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; } = default!;
        
        public required String Code { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public required String UserId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public required String ClientId { get; set; }

        public String? State { get; set; } 

        public String? CodeChallenge { get; set; }
        public String? CodeChallengeMethod { get; set; }

        public DateTime? ExpiresAt { get; set; }
    }
}
