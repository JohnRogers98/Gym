using AutoMapper;
using Gym.Application.Services.CalendarEventApi;
using Gym.Application.Services.CalendarEventApi.CreateCalendarEvent;
using Gym.WebDto.Requests.CalendarEvent;
using Gym.WebDto.Responses.CalendarEvent;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.CalendarEvents
{
    [Route("api/calendar-events")]
    [ApiController]
    public class CreateCalendarEventController(IMediator _mediator, IMapper _mapper) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<CreateCallendarEventResponse>> CreateCalendarEvent(CreateCalendarEventRequest request)
        {
            CalendarEventDetails calendarEventDetails = await _mediator.Send(_mapper.Map<CreateCalendarEventCommand>(request));

            return base.CreatedAtAction(
                nameof(GetCalendarEventController.GetCalendarEvent),
                "GetCalendarEvent",
                new { calendarEventDetails.id },
                _mapper.Map<CreateCallendarEventResponse>(calendarEventDetails));
            
        }
    }
}
