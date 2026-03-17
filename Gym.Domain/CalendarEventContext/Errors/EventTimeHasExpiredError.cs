using Gym.Domain._Common;
using Gym.Domain.CalendarEventContext.ValueObjects;

namespace Gym.Domain.CalendarEventContext.Errors
{
    public class EventTimeHasExpiredError : DomainError
    {
        public CalendarEventId CalendarEventId { get; }

        private EventTimeHasExpiredError(CalendarEventId calendarEventId) : base(nameof(EventTimeHasExpiredError))
        {
            CalendarEventId = calendarEventId;
        }

        public static EventTimeHasExpiredError Create(CalendarEventId calendarEventId) => new(calendarEventId);

        public override String GetErrorMessage() => $"Event time - {CalendarEventId} has expired for booking.";
    }
}
