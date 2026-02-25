using AutoMapper;
using Gym.Application.Services.CalendarEventApi.CancelCalendarEvent;
using Gym.WebApi.Extensions;
using Gym.WebDto.Requests.CalendarEvent;
using Gym.WebDto.Responses.CalendarEvent;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.CalendarEvents.AdminOnly
{
    [Route("api/admin-calendar-events/actions/cancel")]
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.AdminOnly))]
    public class CancelCalendarEventController(IMediator _mediator, IMapper _mapper) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<CancelCalendarEventResponse>> CancelCalendarEvent(CancelCalendarEventRequest request)
        {
            CancelCalendarEventResult cancelCalendarEventResult = await _mediator.Send(_mapper.Map<CancelCalendarEvent>(request));

            return _mapper.Map<CancelCalendarEventResponse>(cancelCalendarEventResult);
        }
    }
}
