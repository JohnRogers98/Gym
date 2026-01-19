using Gym.Domain._Common;
using Gym.Domain._Shared;

namespace Gym.Domain.BookingAggregate.Events
{
    public class TrainingCompletedDomainEvent : DomainEvent
    {
        public UserId UserId { get; private set; }
        public CalendarEventId CalendarEventId { get; private set; }

        private TrainingCompletedDomainEvent(UserId userId, CalendarEventId calendarEventId)
            => (UserId, CalendarEventId) = (userId, calendarEventId);

        public static TrainingCompletedDomainEvent Create(UserId userId, CalendarEventId calendarEventId) => new(userId, calendarEventId);
    }
}
