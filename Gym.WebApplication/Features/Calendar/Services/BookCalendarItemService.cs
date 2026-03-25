using AutoMapper;
using Gym.WebApplication.Features.Calendar.Models;
using Gym.WebApplication.ViewModels;
using Gym.WebDto.Requests.CalendarEvent;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Calendar.Services
{
    public interface IBookCalendarItemService
    {
        Task<Boolean> HandleAsync(ClientCalendarItemViewModel calendarItem, PollResponse? pollResponse, CancellationToken cancellationToken = default);
    }

    public class BookCalendarItemService(HttpClient _httpClient, IMapper _mapper) : IBookCalendarItemService
    {
        public async Task<Boolean> HandleAsync(ClientCalendarItemViewModel calendarItem, PollResponse? pollResponse, CancellationToken cancellationToken = default)
        {
            BookTrainingEventRequest bookTrainingEventRequest = new() 
            { 
                CalendarEventId = calendarItem.Id,
                PollResponse = pollResponse is null ? null :  _mapper.Map<CalendarEventPollResponseDto>(pollResponse)
            };

            var response = await _httpClient.PostAsJsonAsync(
                "api/client-calendar-events/actions/book",
                bookTrainingEventRequest,
                cancellationToken);

            return response.IsSuccessStatusCode;
        }
    }
}
