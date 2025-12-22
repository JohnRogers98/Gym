using AutoMapper;
using Gym.WebApplication.ViewModels;
using Gym.WebDto.Dto;
using Gym.WebDto.Responses;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Calendar.Services
{
    public class CalendarService(HttpClient _httpClient, IMapper _mapper) : ICalendarService
    {
        public async Task<IEnumerable<CalendarItemViewModel>> GetAllCalendarItemsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ListResponse<CalendarEventDto>>("api/calendar-events");
            return _mapper.Map<IEnumerable<CalendarItemViewModel>>(response!.data);
        }
    }
}
