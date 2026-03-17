using Gym.WebApplication.ViewModels;
using Gym.WebDto.Requests.CalendarEvent;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Calendar.Services
{
    public interface IBookCalendarItemService
    {
        Task<Boolean> HandleAsync(ClientCalendarItemViewModel calendarItem, CancellationToken cancellationToken = default);
    }

    public class BookCalendarItemService(HttpClient _httpClient) : IBookCalendarItemService
    {
        public async Task<Boolean> HandleAsync(ClientCalendarItemViewModel calendarItem, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "api/client-calendar-events/actions/book",
                new BookCalendarEventRequest { CalendarEventId = calendarItem.Id },
                cancellationToken);

            return response.IsSuccessStatusCode;
        }
    }
}
