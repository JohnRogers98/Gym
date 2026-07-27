using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gym.AuthorizationServer.Infrastructure.Entities.UserConsents
{
    public class ScopeInfo
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public required String Id { get; set; }
        public required String Name { get; set; }
    }
}
