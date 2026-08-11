using MongoDB.Bson;

namespace Gym.Infrastructure.Entities.Repositories.Instructors
{
    internal class InstructorEntity
    {
        public ObjectId Id { get; set; }
        public required ObjectId UserId { get; set; }
    }
}
