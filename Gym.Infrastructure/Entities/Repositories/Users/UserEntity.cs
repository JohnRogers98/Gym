using MongoDB.Bson;

namespace Gym.Infrastructure.Entities.Repositories.Users
{
    internal class UserEntity
    {
        public ObjectId Id { get; set; }

        public String? FirstName {  get; set; }
        public String? LastName {  get; set; }

        public required String Role { get; set; }
    }
}
