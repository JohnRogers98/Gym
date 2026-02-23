using Gym.Domain._Common;
using Gym.Domain._Shared;

namespace Gym.Domain.AccountContext.Events
{
    public class TrainingBookingCancelledDomainEvent : DomainEvent
    {
        public BookingId BookingId { get; }
        public UserId UserId { get; private set; }
        public CalendarEventId CalendarEventId { get; private set; }

        private TrainingBookingCancelledDomainEvent(BookingId bookingId, UserId userId, CalendarEventId calendarEventId)
            => (BookingId, UserId, CalendarEventId) = (bookingId, userId, calendarEventId);

        public static TrainingBookingCancelledDomainEvent Create(BookingId bookingId, UserId userId, CalendarEventId calendarEventId) 
            => new(bookingId, userId, calendarEventId);
    }
}
