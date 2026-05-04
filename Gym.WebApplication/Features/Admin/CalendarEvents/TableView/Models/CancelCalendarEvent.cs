using Gym.WebApplication.Features.Admin.Shared.ValueObjects;

namespace Gym.WebApplication.Features.Admin.CalendarEvents.TableView.Models
{
    public class CancelCalendarEvent
    {
        public required CalendarEventId CalendarEventId { get; set; }
    }
}
