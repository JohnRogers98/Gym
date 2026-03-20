using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.AccountContext.ValueObjects;
using Gym.Domain.CalendarEventContext.ValueObjects;

namespace Gym.Domain.AccountContext.Events
{
    public class TrainingCompletedDomainEvent : DomainEvent
    {
        public BookingId BookingId { get; }
        public UserId UserId { get; private set; }
        public CalendarEventId CalendarEventId { get; private set; }

        private TrainingCompletedDomainEvent(DomainEventId id, DateTime occurredOn, BookingId bookingId, UserId userId, CalendarEventId calendarEventId)
            : base(id, occurredOn)
            => (BookingId, UserId, CalendarEventId) = (bookingId, userId, calendarEventId);

        public static TrainingCompletedDomainEvent Create(BookingId bookingId, UserId userId, CalendarEventId calendarEventId) 
            => new(DomainEventId.Generate(), DateTime.UtcNow, bookingId, userId, calendarEventId);

        public static TrainingCompletedDomainEvent Restore(DomainEventId id, DateTime occurredOn, BookingId bookingId, UserId userId, CalendarEventId calendarEventId)
            => new(id, occurredOn, bookingId, userId, calendarEventId);
    }
}
