using AutoMapper;
using Gym.Application.Services.CalendarEventApi;
using Gym.Application.Services.CalendarEventApi.CreateCalendarEvent;
using Gym.WebApi.Extensions;
using Gym.WebDto.Requests.CalendarEvent;
using Gym.WebDto.Responses.CalendarEvent;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.CalendarEvents.AdminOnly
{
    [Route("api/admin-calendar-events")]
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.AdminOnly))]
    public class CreateCalendarEventController(IMediator _mediator, IMapper _mapper) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<CreateCalendarEventResponse>> CreateCalendarEvent(CreateCalendarEventRequest request)
        {
            CalendarEventDetails calendarEventDetails = await _mediator.Send(_mapper.Map<CreateCalendarEvent>(request));

            return base.CreatedAtAction(
                nameof(GetCalendarEventController.GetCalendarEvent),
                "GetCalendarEvent",
                new { calendarEventDetails.Id },
                _mapper.Map<CreateCalendarEventResponse>(calendarEventDetails));
            
        }
    }
}
