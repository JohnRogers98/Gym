using Gym.WebApplication.Features.Admin.Shared.ValueObjects;

namespace Gym.WebApplication.Features.Admin.CalendarEvents.States
{
    public interface ICalendarEventCancellationState
    {
        event Action<CalendarEventId>? CalendarEventCancelled;

        void Notify(CalendarEventId calendarEventId);
    }

    public class CalendarEventCancellationState : ICalendarEventCancellationState
    {
        public event Action<CalendarEventId>? CalendarEventCancelled;

        public void Notify(CalendarEventId calendarEventId)
        {
            CalendarEventCancelled?.Invoke(calendarEventId);
        }
    }
}
