using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gym.AuthorizationServer.Infrastructure.Entities.ProtectedResources
{
    public class ProtectedResourceEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; } = default!;

        public required String AudienceUri { get; set; }

        public required String Name { get; set; }
    }
}
