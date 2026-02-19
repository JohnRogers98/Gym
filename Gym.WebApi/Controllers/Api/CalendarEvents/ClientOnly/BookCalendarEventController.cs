using AutoMapper;
using Gym.Application.Services.BookingApi;
using Gym.Application.Services.BookingApi.BookTrainingEvent;
using Gym.WebApi.Extensions;
using Gym.WebDto.Requests.CalendarEvent;
using Gym.WebDto.Responses.Bookings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.CalendarEvents.ClientOnly
{
    [Route("api/client-calendar-events/actions/book")]
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.ClientOnly))]
    public class BookCalendarEventController(IMediator _mediator, IMapper _mapper) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<BookTrainingEventResponse>> BookTrainingEvent(BookTrainingEventRequest request)
        {
            try
            {
                BookingDetails bookingDetails = await _mediator.Send(
                    new BookTrainingEvent(User.GetRequiredUserId(), request.CalendarEventId));

                return base.Ok(_mapper.Map<BookTrainingEventResponse>(bookingDetails));
            }
            catch (Exception ex)
            {
                ProblemDetails problemDetails = new()
                {
                    Title = "Booking conflict",
                    Status = StatusCodes.Status409Conflict,
                    Detail = ex.Message,
                    Instance = HttpContext.Request.Path
                };
                problemDetails.Extensions["Code"] = "BOOKING_CONFLICT";

                return Conflict(problemDetails);
            }
        }
    }
}
