using Gym.WebApplication.Features.Calendar.Models;
using Gym.WebApplication.ViewModels;

namespace Gym.WebApplication.Features.Client.Calendar.Models
{
    public class BookCalendarItem
    {
        public required CalendarEventForClientViewModel CalendarItem { get; set; }
        
        public PollResponse? PollResponse { get; set; }
    }
}
