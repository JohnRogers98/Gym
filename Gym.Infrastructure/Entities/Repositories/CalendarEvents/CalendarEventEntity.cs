using MongoDB.Bson;

namespace MongoConsoleApp.Repositories.CalendarEvents
{
    internal class CalendarEventEntity
    {
        public ObjectId Id { get; set; }

        public required DateTime Start {  get; set; }
        public DateTime? End { get; set; }

        public IEnumerable<ObjectId>? Bookings { get; set; }
        
        public Int32? MaxClientCount { get; set; }

        public required ObjectId TrainingId { get; set; }

        public IEnumerable<ObjectId>? Instructors { get; set; }
    }
}
