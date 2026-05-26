using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gym.AuthorizationServer.Entities.Users.FormCredentials
{
    public class FormCredentialEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; } = default!;

        public required String Username { get; set; }

        public required String HashedPassword { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public required String UserId { get; set; }
    }
}
