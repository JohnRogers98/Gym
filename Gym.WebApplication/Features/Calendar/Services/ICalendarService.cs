using Gym.WebApplication.ViewModels;

namespace Gym.WebApplication.Features.Calendar.Services
{
    public interface ICalendarService
    {
        Task<IEnumerable<CalendarItemViewModel>> GetAllCalendarItemsAsync();
    }
}
