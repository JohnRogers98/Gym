using AutoMapper;
using Gym.WebApplication.ViewModels;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.CalendarEvent;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Calendar.Services
{
    public interface IGetAllCalendarItemsService
    {
        Task<IEnumerable<ClientCalendarItemViewModel>> ExecuteAsync(CancellationToken cancellationToken = default);
    }

    public class GetAllCalendarItemsService(HttpClient _httpClient, IMapper _mapper) : IGetAllCalendarItemsService
    {
        public async Task<IEnumerable<ClientCalendarItemViewModel>> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetFromJsonAsync<ListResponse<ClientCalendarEventDto>>("api/client-calendar-events", cancellationToken: cancellationToken);
            return _mapper.Map<IEnumerable<ClientCalendarItemViewModel>>(response!.Data);
        }
    }
}
