using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.AccountContext.ValueObjects;
using Gym.Domain.CalendarEventContext.ValueObjects;

namespace Gym.Domain.AccountContext.Events
{
    public class TrainingCancelledDomainEvent : DomainEvent
    {
        public BookingId BookingId { get; }
        public UserId UserId { get; private set; }
        public CalendarEventId CalendarEventId { get; private set; }

        private TrainingCancelledDomainEvent(DomainEventId id, DateTime occurredOn, BookingId bookingId, UserId userId, CalendarEventId calendarEventId)
            :base(id, occurredOn)
            => (BookingId, UserId, CalendarEventId) = (bookingId, userId, calendarEventId);

        public static TrainingCancelledDomainEvent Create(BookingId bookingId, UserId userId, CalendarEventId calendarEventId) 
            => new(DomainEventId.Generate(), DateTime.Now, bookingId, userId, calendarEventId);

        public static TrainingCancelledDomainEvent Restore(DomainEventId id, DateTime occurredOn, BookingId bookingId, UserId userId, CalendarEventId calendarEventId)
            => new(id, occurredOn, bookingId, userId, calendarEventId);
    }
}
