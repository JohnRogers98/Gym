using AutoMapper;
using Gym.Abstractions.Query.CalendarEvents;
using Gym.Application.Services.CalendarEventApi.GetCalendarEventById;
using Gym.WebApi.Extensions;
using Gym.WebDto.Responses.CalendarEvent;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.CalendarEvents.ClientOnly
{
    [Route("api/client-calendar-events/{id}")]
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.ClientOnly))]
    public class GetCalendarEventController(IMediator _mediator, IMapper _mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<GetClientCalendarEventResponse>> GetCalendarEvent(String id)
        {
            CalendarEventProjection calendarEventProjection = await _mediator.Send(new GetCalendarEventById(id));
            
            var response = _mapper.Map<GetClientCalendarEventResponse>(calendarEventProjection, opts =>
            {
                opts.Items["CurrentUserId"] = User.GetRequiredUserId();
            });

            return base.Ok(response);
        }
    }
}
