using MongoDB.Bson;

namespace Gym.Infrastructure.Entities.Repositories.Users
{
    internal class UserEntity
    {
        public ObjectId Id { get; set; }

        public Int64? TelegramId { get; set; }

        public required String Role { get; set; }
    }
}
