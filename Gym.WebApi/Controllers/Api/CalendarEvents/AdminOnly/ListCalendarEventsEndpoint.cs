using Ardalis.ApiEndpoints;
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
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.AdminOnly))]
    public class ListCalendarEventsEndpoint(IMediator _mediator, IMapper _mapper) : EndpointBaseAsync
        .WithoutRequest
        .WithActionResult<ListResponse<AdminCalendarEventDto>>
    {
        [HttpGet("api/admin-calendar-events")]
        public override async Task<ActionResult<ListResponse<AdminCalendarEventDto>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            IEnumerable<CalendarEventProjection> calendarEventProjections = await _mediator.Send(new GetAllCalendarEvents(), cancellationToken);

            var response = new ListResponse<AdminCalendarEventDto>(_mapper.Map<IEnumerable<AdminCalendarEventDto>>(calendarEventProjections));
            return base.Ok(response);
        }
    }
}
