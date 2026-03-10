using AutoMapper;
using Gym.Abstractions.Query.CalendarEvents;
using Gym.Application.Services.CalendarEventApi.GetAllCalendarEvents;
using Gym.WebApi.Extensions;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.CalendarEvent;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.CalendarEvents.AdminOnly
{
    [Route("api/admin-calendar-events")]
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.AdminOnly))]
    public class ListCalendarEventsController(IMediator _mediator, IMapper _mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ListResponse<AdminCalendarEventDto>>> ListCalendarEvents()
        {
            IEnumerable<CalendarEventProjection> calendarEventProjections = await _mediator.Send(new GetAllCalendarEvents());

            return base.Ok(
                new ListResponse<AdminCalendarEventDto>(
                    _mapper.Map<IEnumerable<AdminCalendarEventDto>>(calendarEventProjections)
                )
            );
        }
    }
}
