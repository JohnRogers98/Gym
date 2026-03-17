using Gym.Domain._Common;
using Gym.Domain.CalendarEventContext.ValueObjects;

namespace Gym.Domain.CalendarEventContext.Errors
{
    public class CalendarEventNotFoundError : DomainError
    {
        public CalendarEventId CalendarEventId { get; }

        private CalendarEventNotFoundError(CalendarEventId calendarEventId) : base(nameof(CalendarEventNotFoundError))
        {
            CalendarEventId = calendarEventId;
        }

        public static CalendarEventNotFoundError Create(CalendarEventId calendarEventId) => new(calendarEventId);

        public override String GetErrorMessage() => $"Calendar event with id - {CalendarEventId.Value} not found.";
    }
}
