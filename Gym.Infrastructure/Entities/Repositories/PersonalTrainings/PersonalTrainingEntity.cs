using MongoDB.Bson;

namespace Gym.Infrastructure.Entities.Repositories.PersonalTrainings
{
    internal class PersonalTrainingEntity
    {
        public ObjectId Id { get; set; }
        public required ObjectId InstructorId { get; set; }
        public required ObjectId ClientId { get; set; }

        public required String Status { get; set; }

        public required DateTime Start { get; set; }
        public DateTime? End { get; set; }

        public required String PaymentStatus { get; set; }

        public String? ClientComment { get; set; }
        public String? InstructorComment { get; set; }
    }
}
