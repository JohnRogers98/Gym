using MongoDB.Bson;

namespace Gym.Infrastructure.Entities.Repositories.TelgramAuths
{
    internal class TelegramAuthEntity
    {
        public Int64 Id { get; set; }

        public ObjectId UserId { get; set; }

        public String? TelegramUsername { get; set; }
    }
}
