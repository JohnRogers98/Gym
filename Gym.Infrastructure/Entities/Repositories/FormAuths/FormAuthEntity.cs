using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gym.Infrastructure.Entities.Repositories.FormAuths
{
    internal class FormAuthEntity
    {
        [BsonId]
        public required String Login { get; set; }

        public required String Password { get; set; }

        public ObjectId UserId { get; set; }
    }
}
