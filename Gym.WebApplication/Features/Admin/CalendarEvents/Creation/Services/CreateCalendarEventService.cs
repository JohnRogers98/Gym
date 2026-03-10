using AutoMapper;
using Gym.WebApplication.Features.Admin.CalendarEvents.Creation.Models.Forms;
using Gym.WebApplication.Features.Admin.CalendarEvents.Creation.Models.Results;
using Gym.WebDto.Requests.CalendarEvent;
using Gym.WebDto.Responses.CalendarEvent;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Admin.CalendarEvents.Creation.Services
{
    public interface ICreateCalendarEventService
    {
        Task<CreateCalendarEventResult> ExecuteAsync(CreateCalendarEventFormModel createCalendarEventFormModel, CancellationToken cancellationToken = default);
    }

    public class CreateCalendarEventService(HttpClient _httpClient, IMapper _mapper) : ICreateCalendarEventService
    {
        public async Task<CreateCalendarEventResult> ExecuteAsync(CreateCalendarEventFormModel createCalendarEventFormModel, CancellationToken cancellationToken = default)
        {
            var createCalendarEventRequest = _mapper.Map<CreateCalendarEventRequest>(createCalendarEventFormModel);

            var response = await _httpClient.PostAsJsonAsync("api/admin-calendar-events", createCalendarEventRequest, cancellationToken);
            var createCalendarEventResponse = await response.Content.ReadFromJsonAsync<CreateCalendarEventResponse>();

            return _mapper.Map<CreateCalendarEventResult>(createCalendarEventResponse);
        }
    }
}
