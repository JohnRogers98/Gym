using MongoDB.Bson;

namespace Gym.Infrastructure.Entities.Repositories.Instructors
{
    public class InstructorEntity
    {
        public ObjectId Id { get; set; }
        public required String FirstName { get; set; }
        public String? LastName { get; set; }
    }
}
