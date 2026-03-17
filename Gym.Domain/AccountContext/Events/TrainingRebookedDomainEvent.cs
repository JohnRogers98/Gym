using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.AccountContext.ValueObjects;
using Gym.Domain.CalendarEventContext.ValueObjects;

namespace Gym.Domain.AccountContext.Events
{
    public class TrainingRebookedDomainEvent : DomainEvent
    {
        public BookingId BookingId { get; }
        public UserId UserId { get; private set; }
        public CalendarEventId CalendarEventId { get; private set; }

        private TrainingRebookedDomainEvent(BookingId bookingId, UserId userId, CalendarEventId calendarEventId)
            => (BookingId, UserId, CalendarEventId) = (bookingId, userId, calendarEventId);

        public static TrainingRebookedDomainEvent Create(BookingId bookingId, UserId userId, CalendarEventId calendarEventId) 
            => new(bookingId, userId, calendarEventId);
    }
}
