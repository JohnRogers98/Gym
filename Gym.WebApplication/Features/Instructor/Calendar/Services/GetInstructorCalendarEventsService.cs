using AutoMapper;
using Gym.WebApplication.Features._Common.Services;
using Gym.WebApplication.Operations;
using Gym.WebApplication.ViewModels;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.CalendarEvent;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Instructor.Calendar.Services
{
    public class GetInstructorCalendarEventsService(HttpClient _httpClient, IMapper _mapper) 
        : IRequestHandler<GetInstructorCalendarEvents, IEnumerable<CalendarEventForAdminViewModel>>
    {
        public async Task<AsyncOperation<IEnumerable<CalendarEventForAdminViewModel>>> HandleAsync(GetInstructorCalendarEvents request, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetFromJsonAsync<ListResponse<AdminCalendarEventDto>>("api/instructors/me/admin-calendar-events", cancellationToken: cancellationToken);

            var responseData = response!.Data.Select(_mapper.Map<CalendarEventForAdminViewModel>).ToList();
            return AsyncOperation<IEnumerable<CalendarEventForAdminViewModel>>.Success(responseData);
        }
    }

    public class GetInstructorCalendarEvents;
}
