using Gym.WebApplication.Features.Admin.CalendarEvents.Creation.Models.Results;

namespace Gym.WebApplication.Features.Admin.CalendarEvents.States
{
    public interface ICalendarEventCreationState
    {
        event Action<CreateCalendarEventResult>? CalendarEventCreated;

        void Notify(CreateCalendarEventResult createCalendarEventResult);
    }

    public class CalendarEventCreationState : ICalendarEventCreationState
    {
        public event Action<CreateCalendarEventResult>? CalendarEventCreated;

        public void Notify(CreateCalendarEventResult createCalendarEventResult)
        {
            CalendarEventCreated?.Invoke(createCalendarEventResult);
        }
    }
}
