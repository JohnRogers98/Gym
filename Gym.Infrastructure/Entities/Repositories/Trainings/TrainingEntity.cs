using MongoDB.Bson;

namespace Gym.Infrastructure.Entities.Repositories.Trainings
{
    internal class TrainingEntity
    {
        public ObjectId Id { get; set; }
        public required String Name { get; set; }
        public String? Description { get; set; }
    }
}
