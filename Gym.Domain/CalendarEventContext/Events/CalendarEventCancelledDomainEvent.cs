using Gym.Domain._Common;
using Gym.Domain._Shared;

namespace Gym.Domain.CalendarEventContext.Events
{
    public class CalendarEventCancelledDomainEvent : DomainEvent
    {
        public CalendarEventId CalendarEventId { get; private set; }
        public IReadOnlyCollection<UserId> BookingUsers { get; private set; }

        private CalendarEventCancelledDomainEvent(DomainEventId id, DateTime occurredOn, CalendarEventId calendarEventId, IReadOnlyCollection<UserId> bookingUsers)
            : base(id, occurredOn)
            => (CalendarEventId, BookingUsers) = (calendarEventId, bookingUsers);

        public static CalendarEventCancelledDomainEvent Create(CalendarEventId calendarEventId, IReadOnlyCollection<UserId> bookingUsers)
            => new(DomainEventId.Generate(), DateTime.Now, calendarEventId, bookingUsers);

        public static CalendarEventCancelledDomainEvent Restore(DomainEventId id, DateTime occurredOn, CalendarEventId calendarEventId, IReadOnlyCollection<UserId> bookingUsers)
           => new(id, occurredOn, calendarEventId, bookingUsers);
    }
}
