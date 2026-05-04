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
    public class GetCalendarEventByIdForAdminService(HttpClient _httpClient, IMapper _mapper) : IRequestHandler<GetCalendarEventByIdForAdmin, CalendarEventForAdminViewModel>
    {
        public async Task<AsyncOperation<CalendarEventForAdminViewModel>> HandleAsync(GetCalendarEventByIdForAdmin request, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync($"api/admin-calendar-events/{request.CalendarEventId.Value}", cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadFromJsonAsync<Response<AdminCalendarEventDto>>(cancellationToken: cancellationToken);
                return AsyncOperation<CalendarEventForAdminViewModel>.Success(_mapper.Map<CalendarEventForAdminViewModel>(responseData!.Data));
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return AsyncOperation<CalendarEventForAdminViewModel>.Failure("Calendar event not found", ErrorType.NotFound);
            }

            return AsyncOperation<CalendarEventForAdminViewModel>.Failure($"Unknown response type.", ErrorType.Unknown, (Int32)response.StatusCode);
        }
    }
}
