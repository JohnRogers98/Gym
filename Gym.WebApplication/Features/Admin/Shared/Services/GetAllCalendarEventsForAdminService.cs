using AutoMapper;
using Gym.WebApplication.Features._Common.Services;
using Gym.WebApplication.Features.Admin.Shared.Models;
using Gym.WebApplication.Operations;
using Gym.WebApplication.ViewModels;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.CalendarEvent;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Admin.Shared.Services
{
    public class GetAllCalendarEventsForAdminService(HttpClient _httpClient, IMapper _mapper) : IRequestHandler<GetAllCalendarEventsForAdmin, IEnumerable<CalendarEventForAdminViewModel>>
    {
        public async Task<AsyncOperation<IEnumerable<CalendarEventForAdminViewModel>>> HandleAsync(GetAllCalendarEventsForAdmin request, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetFromJsonAsync<ListResponse<AdminCalendarEventDto>>("api/admin-calendar-events", cancellationToken: cancellationToken);
            
            var responseData = response!.Data.Select(_mapper.Map<CalendarEventForAdminViewModel>).ToList();
            return AsyncOperation<IEnumerable<CalendarEventForAdminViewModel>>.Success(responseData);
        }
    }
}
