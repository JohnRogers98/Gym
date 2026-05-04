using Gym.WebApplication.Extensions;
using Gym.WebApplication.Features._Common.Services;
using Gym.WebApplication.Features.Admin.CalendarEvents.TableView.Models;
using Gym.WebApplication.Operations;
using Gym.WebDto.Requests.CalendarEvent;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Admin.CalendarEvents.TableView.Services
{

    public class CancelCalendarEventService(HttpClient _httpClient) : IRequestHandler<CancelCalendarEvent, CancelCalendarEventResult>
    {
        public async Task<AsyncOperation<CancelCalendarEventResult>> HandleAsync(CancelCalendarEvent request, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/admin-calendar-events/{request.CalendarEventId.Value}/actions/cancel", new CancelCalendarEventRequest(), cancellationToken);

            if (response.IsSuccessStatusCode)
                return AsyncOperation<CancelCalendarEventResult>.Success(new CancelCalendarEventResult());

            if (response.IsContentTypeProblemDetails())
                return await response.GetFailedOperationFromProblemDetailsAsync<CancelCalendarEventResult>(cancellationToken);

            return AsyncOperation<CancelCalendarEventResult>.Failure($"Unknown response type.", ErrorType.Unknown, (Int32)response.StatusCode);
        }
    }
}
