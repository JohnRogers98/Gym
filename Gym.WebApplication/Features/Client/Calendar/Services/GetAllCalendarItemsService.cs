using AutoMapper;
using Gym.WebApplication.Features._Common.Services;
using Gym.WebApplication.Features.Client.Calendar.Models;
using Gym.WebApplication.Operations;
using Gym.WebApplication.ViewModels;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.CalendarEvent;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Calendar.Services
{
    public class GetAllCalendarItemsService(HttpClient _httpClient, IMapper _mapper) : IRequestHandler<GetAllCalendarItems, IEnumerable<CalendarEventForClientViewModel>>
    {
        public async Task<AsyncOperation<IEnumerable<CalendarEventForClientViewModel>>> HandleAsync(GetAllCalendarItems request, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetFromJsonAsync<ListResponse<ClientCalendarEventDto>>("api/client-calendar-events", cancellationToken: cancellationToken);

            return AsyncOperation<IEnumerable<CalendarEventForClientViewModel>>.Success( 
                _mapper.Map<IEnumerable<CalendarEventForClientViewModel>>(response!.Data));
        }
    }
}
