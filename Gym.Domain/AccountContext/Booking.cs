using Gym.Domain._Exceptions;
using Gym.Domain._Shared;
using Gym.Domain.AccountContext.Errors;

namespace Gym.Domain.AccountContext
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

        internal void Cancel()
        {
            if (Status is not BookingStatus.Upcoming)
            {
                throw new DomainException(IncorrectBookingStatusStateError.Create(Id, Status));
            }

            Status = BookingStatus.Cancelled;
        }

        internal void Rebook()
        {
            if (Status is not BookingStatus.Cancelled)
            {
                throw new DomainException(IncorrectBookingStatusStateError.Create(Id, Status));
            }

            Status = BookingStatus.Upcoming;
        }

        internal void MarkAsCompleted()
        {
            if (Status is not BookingStatus.Upcoming)
            {
                throw new DomainException(IncorrectBookingStatusStateError.Create(Id, Status));
            }

            Status = BookingStatus.Completed;
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
