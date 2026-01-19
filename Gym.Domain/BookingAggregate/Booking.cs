using Gym.Domain._Common;
using Gym.Domain._Exceptions;
using Gym.Domain._Shared;
using Gym.Domain.BookingAggregate.Events;

namespace Gym.Domain.BookingAggregate
{
    public class Booking : AggregateRoot
    {
        public BookingId Id { get; }

        public UserId UserId { get; }

        public CalendarEventId CalendarEventId { get; }

        public DateTime ChangedAt {  get; }
        public BookingStatus Status { get; private set; }

        private Booking(BookingId id, UserId userId, CalendarEventId calendarEventId, DateTime bookingDateTime, BookingStatus bookingStatus)
        {
            Id = id;
            UserId = userId;
            CalendarEventId = calendarEventId;
            ChangedAt = bookingDateTime;
            Status = bookingStatus;
        }

        public static Booking Create(BookingId id, UserId userId, CalendarEventId calendarEventId)
        {
            Booking booking = new(id, userId, calendarEventId, DateTime.UtcNow, BookingStatus.Upcoming);
            booking.AddDomainEvent(TrainingBookedDomainEvent.Create(booking.UserId, booking.CalendarEventId));
            return booking;
        }

        public static Booking Restore(BookingId id, UserId userId, CalendarEventId calendarEventId, DateTime bookingDateTime, BookingStatus bookingStatus)
            => new(id, userId, calendarEventId, bookingDateTime, bookingStatus);

        internal void Cancel()
        {

            if (Status != BookingStatus.Upcoming)
            {
                throw new DomainException($"Current booking status - {Status} incorrect for operation.");
            }

            Status = BookingStatus.Cancelled;
            base.AddDomainEvent(TrainingBookingCancelledDomainEvent.Create(UserId, CalendarEventId));
        }

        internal void Rebook()
        {
            if (Status != BookingStatus.Cancelled)
            {
                throw new DomainException($"Current booking status - {Status} incorrect for operation.");
            }

            Status = BookingStatus.Upcoming;
            base.AddDomainEvent(TrainingRebookedDomainEvent.Create(UserId, CalendarEventId));
        }

        internal void MarkAsCompleted()
        {
            if (Status != BookingStatus.Upcoming)
            {
                throw new DomainException($"Current booking status - {Status} incorrect for operation.");
            }

            Status = BookingStatus.Completed;
            base.AddDomainEvent(TrainingCompletedDomainEvent.Create(UserId, CalendarEventId));
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
