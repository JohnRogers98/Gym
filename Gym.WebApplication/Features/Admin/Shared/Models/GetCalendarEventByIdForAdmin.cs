using Gym.WebApplication.Features.Admin.Shared.ValueObjects;

namespace Gym.WebApplication.Features.Admin.Shared.Models
{
    public class GetCalendarEventByIdForAdmin
    {
        public required CalendarEventId CalendarEventId { get; set; }
    }
}
