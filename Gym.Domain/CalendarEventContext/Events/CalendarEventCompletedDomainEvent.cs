using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.CalendarEventContext.ValueObjects;

namespace Gym.Domain.CalendarEventContext.Events
{
    public class CalendarEventCompletedDomainEvent : DomainEvent
    {
        public CalendarEventId CalendarEventId { get; private set; }
        public IReadOnlyCollection<UserId> BookingUsers { get; private set; }

        private CalendarEventCompletedDomainEvent(DomainEventId id, DateTime occurredOn, CalendarEventId calendarEventId, IReadOnlyCollection<UserId> bookingUsers)
            : base(id, occurredOn)
            => (CalendarEventId, BookingUsers) = (calendarEventId, bookingUsers);

        public static CalendarEventCompletedDomainEvent Create(CalendarEventId calendarEventId, IReadOnlyCollection<UserId> bookingUsers)
            => new(DomainEventId.Generate(), DateTime.Now, calendarEventId, bookingUsers);

        public static CalendarEventCompletedDomainEvent Restore(DomainEventId id, DateTime occurredOn, CalendarEventId calendarEventId, IReadOnlyCollection<UserId> bookingUsers)
           => new(id, occurredOn, calendarEventId, bookingUsers);
    }
}
