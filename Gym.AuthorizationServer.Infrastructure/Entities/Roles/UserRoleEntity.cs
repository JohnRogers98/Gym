using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gym.AuthorizationServer.Infrastructure.Entities.Roles
{
    public class UserRoleEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; } = default!;

        public required String Name { get; set; }
    }
}
