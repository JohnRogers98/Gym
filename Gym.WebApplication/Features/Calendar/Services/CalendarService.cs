using AutoMapper;
using Gym.WebApplication.ViewModels;
using Gym.WebDto.Dto;
using Gym.WebDto.Requests.CalendarEvent;
using Gym.WebDto.Responses;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Calendar.Services
{
    public interface ICalendarService
    {
        Task<IEnumerable<CalendarItemViewModel>> GetAllCalendarItemsAsync();
        Task<Boolean> BookCalendarItem(CalendarItemViewModel calendarItem);
    }

    public class CalendarService(HttpClient _httpClient, IMapper _mapper) : ICalendarService
    {
        public async Task<IEnumerable<CalendarItemViewModel>> GetAllCalendarItemsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ListResponse<ClientCalendarEventDto>>("api/client-calendar-events");
            return _mapper.Map<IEnumerable<CalendarItemViewModel>>(response!.Data);
        }

        public async Task<Boolean> BookCalendarItem(CalendarItemViewModel calendarItem)
        {
            var response = await _httpClient.PostAsJsonAsync("api/client-calendar-events/actions/book", new BookCalendarEventRequest { CalendarEventId = calendarItem.Id });

            return response.IsSuccessStatusCode;
        }
    }
}
