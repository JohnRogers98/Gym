using Gym.Domain._Common;
using Gym.Domain._Shared;

namespace Gym.Domain.BookingAggregate.Events
{
    public class TrainingBookedDomainEvent : DomainEvent
    {
        public UserId UserId { get; private set; }
        public CalendarEventId CalendarEventId { get; private set; }

        private TrainingBookedDomainEvent(UserId userId, CalendarEventId calendarEventId) 
            => (UserId, CalendarEventId) = (userId, calendarEventId);

        public static TrainingBookedDomainEvent Create(UserId userId, CalendarEventId calendarEventId) => new(userId, calendarEventId);
    }
}
