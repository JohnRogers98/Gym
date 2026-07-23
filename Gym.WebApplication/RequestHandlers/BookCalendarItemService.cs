using AutoMapper;
using Gym.WebApplication.Extensions;
using Gym.WebApplication.Features._Common.Services;
using Gym.WebApplication.Operations;
using Gym.WebApplication.Options;
using Gym.WebApplication.ViewModels;
using Gym.WebDto.Requests.CalendarEvent;
using Microsoft.Extensions.Options;

namespace Gym.WebApplication.RequestHandlers
{
    public class BookCalendarItemService(IHttpClientFactory _httpClientFactory, IOptions<BffOptions> _bffOptions, IMapper _mapper) 
        : IRequestHandler<BookCalendarItem, BookCalendarItemResult>
    {
        public async Task<AsyncOperation<BookCalendarItemResult>> HandleAsync(BookCalendarItem request, CancellationToken cancellationToken = default)
        {
            BookTrainingEventRequest bookTrainingEventRequestObj = new()
            {
                CalendarEventId = request.CalendarItem.Id,
                PollResponse = request.PollResponse is null ? null : _mapper.Map<CalendarEventPollResponseDto>(request.PollResponse)
            };

            using var httpClient = _httpClientFactory.CreateClient(_bffOptions.Value.ClientName);

            using var bookingRequest = this.CreatePostRequestWithJson(_bffOptions.Value.BookCalendarEventEndpoint, bookTrainingEventRequestObj);

            var bookingResponse = await httpClient.SendAsync(bookingRequest, cancellationToken);
            if (bookingResponse.IsSuccessStatusCode)
            {
                return AsyncOperation<BookCalendarItemResult>.Success(new BookCalendarItemResult());
            }

            if (bookingResponse.IsContentTypeProblemDetails())
            {
                return await bookingResponse.GetFailedOperationFromProblemDetailsAsync<BookCalendarItemResult>(cancellationToken);
            }

            return AsyncOperation<BookCalendarItemResult>.UnknownResponseType((Int32)bookingResponse.StatusCode);
        }
    }

    public class BookCalendarItem
    {
        public required CalendarEventForClientViewModel CalendarItem { get; set; }

        public PollResponse? PollResponse { get; set; }
    }

    public record PollResponse
    {
        public required String PollId { get; init; }
        public required IReadOnlyCollection<Int32> SelectedChoices { get; init; }
    }

    public class BookCalendarItemResult;
}
