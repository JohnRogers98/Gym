using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.AccountContext.Errors;
using Gym.Domain.AccountContext.ValueObjects;
using Gym.Domain.CalendarEventContext.ValueObjects;

namespace Gym.Domain.AccountContext.Entities
{
    public class Booking
    {
        public BookingId Id { get; }

        public UserId UserId { get; }

        public CalendarEventId CalendarEventId { get; }

        public BookingStatus Status { get; private set; }

        private Booking(BookingId id, UserId userId, CalendarEventId calendarEventId, BookingStatus bookingStatus)
        {
            Id = id;
            UserId = userId;
            CalendarEventId = calendarEventId;
            Status = bookingStatus;
        }

        public static Booking Create(BookingId id, UserId userId, CalendarEventId calendarEventId)
            => new(id, userId, calendarEventId, BookingStatus.Upcoming);

        public static Booking Restore(BookingId id, UserId userId, CalendarEventId calendarEventId, BookingStatus bookingStatus)
            => new(id, userId, calendarEventId, bookingStatus);

        internal Result Cancel()
        {
            if (Status is not BookingStatus.Upcoming)
            {
                return Result.Fail(IncorrectBookingStatusStateError.Create(Id, Status));
            }

            Status = BookingStatus.Cancelled;

            return Result.Ok();
        }

        internal Result Rebook()
        {
            if (Status is not BookingStatus.Cancelled)
            {
                return Result.Fail(IncorrectBookingStatusStateError.Create(Id, Status));
            }

            Status = BookingStatus.Upcoming;

            return Result.Ok();
        }

        internal Result MarkAsCompleted()
        {
            if (Status is not BookingStatus.Upcoming)
            {
                return Result.Fail(IncorrectBookingStatusStateError.Create(Id, Status));
            }

            Status = BookingStatus.Completed;

            return Result.Ok();
        }

        public override String ToString()
            => $"{nameof(Id)}: {Id} \t {nameof(UserId)}: {UserId}";

        public override Boolean Equals(Object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj is Booking other && Id == other.Id;
        }

        public override Int32 GetHashCode() => Id.GetHashCode();
    }
}
