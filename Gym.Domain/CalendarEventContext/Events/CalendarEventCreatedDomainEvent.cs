using Gym.Domain._Common;
using Gym.Domain._Shared;

namespace Gym.Domain.CalendarEventContext.Events
{
    public class CalendarEventCreatedDomainEvent : DomainEvent
    {
        public CalendarEventId CalendarEventId { get; private set; }

        private CalendarEventCreatedDomainEvent(DomainEventId id, DateTime occurredOn, CalendarEventId calendarEventId)
            : base(id, occurredOn)
            => (CalendarEventId) = (calendarEventId);

        public static CalendarEventCreatedDomainEvent Create(CalendarEventId calendarEventId)
            => new(DomainEventId.Generate(), DateTime.Now, calendarEventId);

        public static CalendarEventCreatedDomainEvent Restore(DomainEventId id, DateTime occurredOn, CalendarEventId calendarEventId)
           => new(id, occurredOn, calendarEventId);
    }
}
