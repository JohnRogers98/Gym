using AutoMapper;
using Gym.WebApplication.Features._Common.Services;
using Gym.WebApplication.Operations;
using Gym.WebApplication.ViewModels;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.CalendarEvent;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Client.Schedule.Services
{
    public class GetClientCalendarEventsService(HttpClient _httpClient, IMapper _mapper) 
        : IRequestHandler<GetClientCalendarEvents, IEnumerable<CalendarEventForClientViewModel>>
    {
        public async Task<AsyncOperation<IEnumerable<CalendarEventForClientViewModel>>> HandleAsync(GetClientCalendarEvents request, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetFromJsonAsync<ListResponse<ClientCalendarEventDto>>("api/clients/me/client-calendar-events", cancellationToken: cancellationToken);

            var responseData = response!.Data.Select(_mapper.Map<CalendarEventForClientViewModel>).ToList();
            return AsyncOperation<IEnumerable<CalendarEventForClientViewModel>>.Success(responseData);
        }
    }

    public class GetClientCalendarEvents;
}
