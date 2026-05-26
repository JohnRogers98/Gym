using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gym.AuthorizationServer.Entities.Users.TelegramCredentials
{
    public class TelegramCredentialEntity
    {
        [BsonId]
        public Int64 Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public required String UserId { get; set; }

        public String? TelegramUsername { get; set; }
    }
}
