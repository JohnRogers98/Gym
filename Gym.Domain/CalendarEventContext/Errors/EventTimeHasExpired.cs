using Gym.Domain._Common;
using Gym.Domain._Shared;

namespace Gym.Domain.CalendarEventContext.Errors
{
    public class EventTimeHasExpired : DomainError
    {
        public CalendarEventId CalendarEventId { get; }

        private EventTimeHasExpired(CalendarEventId calendarEventId) : base(nameof(EventTimeHasExpired))
        {
            CalendarEventId = calendarEventId;
        }

        public static EventTimeHasExpired Create(CalendarEventId calendarEventId) => new(calendarEventId);

        public override String GetErrorMessage() => $"Event time - {CalendarEventId} has expired for booking.";
    }
}
