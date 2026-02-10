using Gym.Domain._Common;
using Gym.Domain._Shared;

namespace Gym.Domain.CalendarEventContext.Errors
{
    public class EventHasNotFreeSpaceError : DomainError
    {
        public CalendarEventId CalendarEventId { get; }

        private EventHasNotFreeSpaceError(CalendarEventId calendarEventId) : base(nameof(EventHasNotFreeSpaceError))
        {
            CalendarEventId = calendarEventId;
        }

        public static EventHasNotFreeSpaceError Create(CalendarEventId calendarEventId) => new(calendarEventId);

        public override String GetErrorMessage() => $"Event - {CalendarEventId} has not free space.";
    }
}
