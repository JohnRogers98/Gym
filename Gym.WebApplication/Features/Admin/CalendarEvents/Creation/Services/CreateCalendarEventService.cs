using AutoMapper;
using Gym.WebApplication.Extensions;
using Gym.WebApplication.Features._Common.Services;
using Gym.WebApplication.Features.Admin.CalendarEvents.Creation.Models.Forms;
using Gym.WebApplication.Features.Admin.CalendarEvents.Creation.Models.Results;
using Gym.WebApplication.Operations;
using Gym.WebDto.Requests.CalendarEvent;
using Gym.WebDto.Responses.CalendarEvent;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Admin.CalendarEvents.Creation.Services
{
    public class CreateCalendarEventService(HttpClient _httpClient, IMapper _mapper) : IRequestHandler<CreateCalendarEventFormModel, CreateCalendarEventResult>
    {
        public async Task<AsyncOperation<CreateCalendarEventResult>> HandleAsync(CreateCalendarEventFormModel request, CancellationToken cancellationToken = default)
        {
            var createCalendarEventRequest = _mapper.Map<CreateCalendarEventRequest>(request);

            var response = await _httpClient.PostAsJsonAsync("api/admin-calendar-events", createCalendarEventRequest, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var createCalendarEventResponse = await response.Content.ReadFromJsonAsync<CreateCalendarEventResponse>();

                return AsyncOperation<CreateCalendarEventResult>
                    .Success(_mapper.Map<CreateCalendarEventResult>(createCalendarEventResponse));
            }

            if (response.IsContentTypeProblemDetails())
                return await response.GetFailedOperationFromProblemDetailsAsync<CreateCalendarEventResult>();

            return AsyncOperation<CreateCalendarEventResult>.Failure($"Unknown response type.", ErrorType.Unknown, (Int32)response.StatusCode);
        }
    }
}
