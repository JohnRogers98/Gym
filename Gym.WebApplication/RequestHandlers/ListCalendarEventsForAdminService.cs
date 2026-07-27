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
    public class ListCalendarEventsForAdminService(IHttpClientFactory _httpClientFactory, IOptions<BffOptions> _bffOptions, IMapper _mapper) : IRequestHandler<ListCalendarEventsForAdmin, IEnumerable<CalendarEventForAdminViewModel>>
    {
        public async Task<AsyncOperation<IEnumerable<CalendarEventForAdminViewModel>>> HandleAsync(ListCalendarEventsForAdmin request, CancellationToken cancellationToken = default)
        {
            using var httpClient = _httpClientFactory.CreateClient(_bffOptions.Value.ClientName);

            using var listCalendarEventsRequest = this.CreateGetRequest(_bffOptions.Value.ListCalendarEventsForAdminEndpoint);

            HttpResponseMessage listCalendarEventsResponse = await httpClient.SendAsync(listCalendarEventsRequest, cancellationToken);
            if (listCalendarEventsResponse.IsSuccessStatusCode)
            {
                var deserializedResponse = await listCalendarEventsResponse.Content.ReadFromJsonAsync<ListResponse<AdminCalendarEventDto>>();
                if (deserializedResponse is null)
                    return AsyncOperation<IEnumerable<CalendarEventForAdminViewModel>>.EmptyResponseBody();

                var calendarItems = deserializedResponse.Data.Select(_mapper.Map<CalendarEventForAdminViewModel>);
                return AsyncOperation<IEnumerable<CalendarEventForAdminViewModel>>.Success(calendarItems);
            }

            if (listCalendarEventsResponse.IsContentTypeProblemDetails())
            {
                return await listCalendarEventsResponse.GetFailedOperationFromProblemDetailsAsync<IEnumerable<CalendarEventForAdminViewModel>>(cancellationToken);
            }

            return AsyncOperation<IEnumerable<CalendarEventForAdminViewModel>>.UnknownResponseType((Int32)listCalendarEventsResponse.StatusCode);
        }
    }

    public class ListCalendarEventsForAdmin;
}
