using Gym.Domain._Common;
using Gym.Domain._Shared;

namespace Gym.Domain.AccountContext.Events
{
    public class TrainingBookedDomainEvent : DomainEvent
    {
        public BookingId BookingId { get; }
        public UserId UserId { get; }
        public CalendarEventId CalendarEventId { get; }

        private TrainingBookedDomainEvent(DomainEventId id, DateTime occurredOn, BookingId bookingId, UserId userId, CalendarEventId calendarEventId)
           : base(id, occurredOn)
           => (BookingId, UserId, CalendarEventId) = (bookingId, userId, calendarEventId);

        public static TrainingBookedDomainEvent Create(BookingId bookingId, UserId userId, CalendarEventId calendarEventId) 
            => new(DomainEventId.Generate(), DateTime.Now, bookingId, userId, calendarEventId);

        public static TrainingBookedDomainEvent Restore(DomainEventId id, DateTime occurredOn, BookingId bookingId, UserId userId, CalendarEventId calendarEventId)
            => new(id, occurredOn, bookingId, userId, calendarEventId);
    }
}
