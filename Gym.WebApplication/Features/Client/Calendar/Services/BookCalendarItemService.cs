using AutoMapper;
using Gym.WebApplication.Extensions;
using Gym.WebApplication.Features._Common.Services;
using Gym.WebApplication.Features.Client.Calendar.Models;
using Gym.WebApplication.Operations;
using Gym.WebDto.Requests.CalendarEvent;
using System.Net.Http.Json;

namespace Gym.WebApplication.Features.Calendar.Services
{
    public class BookCalendarItemService(HttpClient _httpClient, IMapper _mapper) : IRequestHandler<BookCalendarItem, BookCalendarItemResult>
    {
        public async Task<AsyncOperation<BookCalendarItemResult>> HandleAsync(BookCalendarItem request, CancellationToken cancellationToken = default)
        {
            BookTrainingEventRequest bookTrainingEventRequest = new() 
            { 
                CalendarEventId = request.CalendarItem.Id,
                PollResponse = request.PollResponse is null ? null :  _mapper.Map<CalendarEventPollResponseDto>(request.PollResponse)
            };

            var response = await _httpClient.PostAsJsonAsync(
                "api/client-calendar-events/actions/book",
                bookTrainingEventRequest,
                cancellationToken);

            if (response.IsContentTypeProblemDetails())
                return await response.GetFailedOperationFromProblemDetailsAsync<BookCalendarItemResult>(cancellationToken);

            if (response.IsSuccessStatusCode)
                return AsyncOperation<BookCalendarItemResult>.Success(new BookCalendarItemResult());

            return AsyncOperation<BookCalendarItemResult>.Failure($"Unknown response type.", ErrorType.Unknown, (Int32)response.StatusCode);
        }
    }
}
