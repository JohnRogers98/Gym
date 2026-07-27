using Gym.WebApplication.Extensions;
using Gym.WebApplication.Features._Common.Services;
using Gym.WebApplication.Operations;
using Gym.WebApplication.Options;
using Gym.WebDto.Requests.CalendarEvent;
using Microsoft.Extensions.Options;

namespace Gym.WebApplication.RequestHandlers
{

    public class CancelCalendarEventService(IHttpClientFactory _httpClientFactory, IOptions<BffOptions> _bffOptions) 
        : IRequestHandler<CancelCalendarEvent, CancelCalendarEventResult>
    {
        public async Task<AsyncOperation<CancelCalendarEventResult>> HandleAsync(CancelCalendarEvent request, CancellationToken cancellationToken = default)
        {
            using var httpClient = _httpClientFactory.CreateClient(_bffOptions.Value.ClientName);

            var endpointUrl = UrlHelper.ReplacePathVariables(_bffOptions.Value.CancelCalendarEventEndpoint, new() { ["calendarEventId"] = request.CalendarEventId! });

            using var cancelCalendarEventRequest = this.CreatePostRequestWithJson(endpointUrl, new CancelCalendarEventRequest());

            var cancelCalendarEventResponse = await httpClient.SendAsync(cancelCalendarEventRequest, cancellationToken);
            if (cancelCalendarEventResponse.IsSuccessStatusCode)
            {
                return AsyncOperation<CancelCalendarEventResult>.Success(new());
            }

            if (cancelCalendarEventResponse.IsContentTypeProblemDetails())
            {
                return await cancelCalendarEventResponse.GetFailedOperationFromProblemDetailsAsync<CancelCalendarEventResult>(cancellationToken);
            }

            return AsyncOperation<CancelCalendarEventResult>.UnknownResponseType((Int32)cancelCalendarEventResponse.StatusCode);
        }
    }

    public class CancelCalendarEvent
    {
        public required String CalendarEventId { get; set; }
    }

    public class CancelCalendarEventResult;
}
