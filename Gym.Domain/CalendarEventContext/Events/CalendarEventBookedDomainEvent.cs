using Gym.Domain._Common;
using Gym.Domain._Shared;

namespace Gym.Domain.CalendarEventContext.Events
{
    public class CalendarEventBookedDomainEvent : DomainEvent
    {
        public CalendarEventId CalendarEventId { get; private set; }
        public UserId UserId { get; private set; }

        private CalendarEventBookedDomainEvent(DomainEventId id, DateTime occurredOn, CalendarEventId calendarEventId, UserId userId)
            : base(id, occurredOn)
            => (CalendarEventId, UserId) = (calendarEventId, userId);

        public static CalendarEventBookedDomainEvent Create(CalendarEventId calendarEventId, UserId userId)
            => new(DomainEventId.Generate(), DateTime.Now, calendarEventId, userId);

        public static CalendarEventBookedDomainEvent Restore(DomainEventId id, DateTime occurredOn, CalendarEventId calendarEventId, UserId userId)
           => new(id, occurredOn, calendarEventId, userId);
    }
}
