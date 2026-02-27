using AutoMapper;
using Gym.Abstractions.Query.CalendarEvents;
using Gym.Application.Services.CalendarEventApi.GetAllCalendarEvents;
using Gym.WebApi.Extensions;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.CalendarEvent;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.CalendarEvents.ClientOnly
{
    [Route("api/client-calendar-events")]
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.ClientOnly))]
    public class ListCalendarEventsController(IMediator _mediator, IMapper _mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ListResponse<ClientCalendarEventDto>>> ListClientCalendarEvents()
        {
            IEnumerable<CalendarEventProjection> calendarEventProjections = await _mediator.Send(new GetAllCalendarEvents());
            
            var response = _mapper.Map<IEnumerable<ClientCalendarEventDto>>(calendarEventProjections, opts =>
            {
                opts.Items["CurrentUserId"] = User.GetRequiredUserId();
            });

            return base.Ok(new ListResponse<ClientCalendarEventDto>(response));
        }
    }
}
