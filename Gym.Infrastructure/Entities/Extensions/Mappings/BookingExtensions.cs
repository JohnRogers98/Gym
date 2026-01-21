using Gym.Domain._Shared;
using Gym.Domain.BookingAggregate;
using Gym.Infrastructure.Entities.Repositories.Bookings;

namespace Gym.Infrastructure.Entities.Extensions.Mappings
{
    internal static class BookingExtensions
    {
        public static Booking ToDomain(this BookingEntity entity)
        {
            var isParsed = Enum.TryParse<BookingStatus>(entity.Status, true, out BookingStatus status);
            if (!isParsed)
            {
                throw new ArgumentException($"Failed to parse status for booking {entity.Id}");
            }

            return Booking.Restore(
                BookingId.From(entity.Id.ToString()),
                UserId.From(entity.UserId.ToString()),
                CalendarEventId.From(entity.CalendarEventId.ToString()),
                entity.ChangedAt,
                status);
        }

        public static BookingEntity ToEntity(this Booking booking)
        {
            return new() 
            { 
                Id = booking.Id.Value.ToObjectId(),
                UserId = booking.UserId.Value.ToObjectId(),
                CalendarEventId = booking.CalendarEventId.Value.ToObjectId(),
                Status = booking.Status.ToString()
            };
        }
    }
}
