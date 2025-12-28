using Gym.Infrastructure.Entities.Repositories.Instructors;
using Gym.Infrastructure.Entities.Repositories.Trainings;
using MongoDB.Bson;

namespace MongoConsoleApp.Repositories.CalendarEvents
{
    internal class CalendarEventEntity
    {
        public ObjectId Id { get; set; }

        public required DateTime Start {  get; set; }
        public DateTime? End { get; set; }
        
        public Int32? MaxClientCount { get; set; }

        public required TrainingEntity Training { get; set; }

        public IEnumerable<InstructorEntity>? Instructors { get; set; }
    }
}
