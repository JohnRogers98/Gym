using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gym.AuthorizationServer.Infrastructure.Entities.Scopes
{
    public class ScopeEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; } = default!;

        public required String Name { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public required String RoleId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public required String ProtectedResourceId { get; set; }
    }
}
