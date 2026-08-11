using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gym.AuthorizationServer.Infrastructure.Entities.Users
{
    public class UserEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; } = default!;

        public String? FirstName { get; set; }

        public String? LastName { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public required String RoleId { get; set; }
    }
}
