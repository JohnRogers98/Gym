using AutoMapper;
using Gym.WebApplication.Extensions;
using Gym.WebApplication.Features._Common.Services;
using Gym.WebApplication.Operations;
using Gym.WebApplication.Options;
using Gym.WebApplication.ViewModels;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.CalendarEvent;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace Gym.WebApplication.RequestHandlers
{
    public class ListSessionClientCalendarEventsService(IHttpClientFactory _httpClientFactory, IOptions<BffOptions> _bffOptions, IMapper _mapper) 
        : IRequestHandler<ListSessionClientCalendarEvents, IEnumerable<CalendarEventForClientViewModel>>
    {
        public async Task<AsyncOperation<IEnumerable<CalendarEventForClientViewModel>>> HandleAsync(ListSessionClientCalendarEvents request, CancellationToken cancellationToken = default)
        {
            using var httpClient = _httpClientFactory.CreateClient(_bffOptions.Value.ClientName);

            using var listCalendarEventsRequest = this.CreateGetRequest(_bffOptions.Value.ListSessionClientCalendarEventsEndpoint);

            HttpResponseMessage listCalendarEventsResponse = await httpClient.SendAsync(listCalendarEventsRequest, cancellationToken);
            if (listCalendarEventsResponse.IsSuccessStatusCode)
            {
                var deserializedResponse = await listCalendarEventsResponse.Content.ReadFromJsonAsync<ListResponse<ClientCalendarEventDto>>();
                if (deserializedResponse is null)
                    return AsyncOperation<IEnumerable<CalendarEventForClientViewModel>>.EmptyResponseBody();

                var calendarItems = deserializedResponse.Data.Select(_mapper.Map<CalendarEventForClientViewModel>);
                return AsyncOperation<IEnumerable<CalendarEventForClientViewModel>>.Success(calendarItems);
            }

            if (listCalendarEventsResponse.IsContentTypeProblemDetails())
            {
                return await listCalendarEventsResponse.GetFailedOperationFromProblemDetailsAsync<IEnumerable<CalendarEventForClientViewModel>>(cancellationToken);
            }

            return AsyncOperation<IEnumerable<CalendarEventForClientViewModel>>.UnknownResponseType((Int32)listCalendarEventsResponse.StatusCode);
        }
    }

    public class ListSessionClientCalendarEvents;
}
