using MongoDB.Bson;

namespace Gym.Infrastructure.Entities.Repositories.Clients
{
    internal class ClientEntity
    {
        public ObjectId Id { get; set; }
        public ObjectId UserId { get; set; }
    }
}
