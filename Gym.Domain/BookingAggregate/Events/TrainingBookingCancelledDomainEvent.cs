using Gym.Domain._Common;
using Gym.Domain._Shared;

namespace Gym.Domain.BookingAggregate.Events
{
    public class TrainingBookingCancelledDomainEvent : DomainEvent
    {
        public UserId UserId { get; private set; }
        public CalendarEventId CalendarEventId { get; private set; }

        private TrainingBookingCancelledDomainEvent(UserId userId, CalendarEventId calendarEventId)
            => (UserId, CalendarEventId) = (userId, calendarEventId);

        public static TrainingBookingCancelledDomainEvent Create(UserId userId, CalendarEventId calendarEventId) => new(userId, calendarEventId);
    }
}
