using Gym.Domain._Common;
using Gym.Domain.CalendarEventContext.ValueObjects;

namespace Gym.Domain.CalendarEventContext.Errors
{
    public class EventStatusIncorrectForOperationError : DomainError
    {
        public CalendarEventId CalendarEventId { get; }

        private EventStatusIncorrectForOperationError(CalendarEventId calendarEventId) : base(nameof(EventStatusIncorrectForOperationError))
        {
            CalendarEventId = calendarEventId;
        }

        public static EventStatusIncorrectForOperationError Create(CalendarEventId calendarEventId) => new(calendarEventId);

        public override String GetErrorMessage() => $"Event  - {CalendarEventId} status incorrect for operation.";
    }
}
