using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gym.AuthorizationServer.Infrastructure.Entities.Clients
{
    public class ClientEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; } = default!;

        public required String SecretHash { get; set; }

        public required String Name { get; set; }

        public required String RedirectUri { get; set; }
    }
}
