using Gym.WebApplication.Features.Admin.Shared.ValueObjects;
using Gym.WebDto.Requests.CalendarEvent;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Admin.CalendarEvents.TableView.Services
{
    public interface ICancelCalendarEventService
    {
        Task<Boolean> HandleAsync(CalendarEventId calendarEventId, CancellationToken cancellationToken = default);
    }

    public class CancelCalendarEventService(HttpClient _httpClient) : ICancelCalendarEventService
    {
        public async Task<Boolean> HandleAsync(CalendarEventId calendarEventId, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/admin-calendar-events/{calendarEventId.Value}/actions/cancel", new CancelCalendarEventRequest(), cancellationToken);
            
            if (response != null && response.IsSuccessStatusCode)
                return true;
            
            return false;
        }
    }
}
