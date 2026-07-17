using Ardalis.ApiEndpoints;
using AutoMapper;
using Gym.Application.Services.BookingApi.BookTrainingEvent;
using Gym.Domain._Common;
using Gym.Domain.AccountContext.Errors;
using Gym.Domain.CalendarEventContext.Errors;
using Gym.Domain.ClientContext.Errors;
using Gym.WebApi.Extensions;
using Gym.WebDto.Requests.CalendarEvent;
using Gym.WebDto.Responses.Bookings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Gym.WebApi.Controllers.Api.CalendarEvents.ClientOnly
{
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.Client))]
    public class BookCalendarEventEndpoint(IMediator _mediator, IMapper _mapper) : EndpointBaseAsync
        .WithRequest<BookTrainingEventRequest>
        .WithActionResult<BookTrainingEventResponse>
    {
        [HttpPost("api/client-calendar-events/actions/book")]
        public override async Task<ActionResult<BookTrainingEventResponse>> HandleAsync(BookTrainingEventRequest request, CancellationToken cancellationToken = default)
        {
            var bookTrainingEvent = new BookTrainingEvent(
                User.GetRequiredUserId(),
                request.CalendarEventId,
                _mapper.Map<CalendarEventPollResponse>(request.PollResponse)
            );

            Result<BookTrainingEventResult> bookTrainingEventResult = await _mediator.Send(bookTrainingEvent, cancellationToken);
            
            if (bookTrainingEventResult.Success)
            {
                return base.Ok(_mapper.Map<BookTrainingEventResponse>(bookTrainingEventResult.Data));
            }

            return bookTrainingEventResult.Error switch
            {
                ClientIdValidationError 
                or AccountNotChargedError
                or ClientNotFoundByUserIdError
                or CalendarEventNotFoundError => this.BadRequestProblem(bookTrainingEventResult.Error.GetErrorMessage()),

                _ => this.InternalErrorProblem(bookTrainingEventResult.Error!.GetErrorMessage())
            };
        }

    }
}
