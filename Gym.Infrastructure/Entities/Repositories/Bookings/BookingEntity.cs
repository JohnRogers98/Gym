using MongoDB.Bson;

namespace Gym.Infrastructure.Entities.Repositories.Bookings
{
    internal class BookingEntity
    {
        public ObjectId Id { get; set; }

        public required ObjectId UserId { get; set; }

        public required ObjectId CalendarEventId { get; set; }

        public DateTime ChangedAt { get; }
        public required String Status { get; set; }
    }
}
