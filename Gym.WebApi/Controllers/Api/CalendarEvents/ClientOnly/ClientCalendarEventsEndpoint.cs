using Ardalis.ApiEndpoints;
using AutoMapper;
using Gym.Abstractions.Query.CalendarEvents;
using Gym.Application.Services.CalendarEventApi.GetCalendarEventsByClientId;
using Gym.WebApi.Extensions;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.CalendarEvent;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Gym.WebApi.Controllers.Api.CalendarEvents.ClientOnly
{
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.Client))]
    public class ClientCalendarEventsEndpoint(IMediator _mediator, IMapper _mapper) : EndpointBaseAsync
        .WithoutRequest
        .WithActionResult<ListResponse<ClientCalendarEventDto>>
    {
        [HttpGet("api/clients/me/client-calendar-events")]
        public override async Task<ActionResult<ListResponse<ClientCalendarEventDto>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            IEnumerable<CalendarEventProjection> calendarEventProjections = 
                await _mediator.Send(new GetCalendarEventsByClientId(User.GetRequiredUserId()), cancellationToken);

            var response = _mapper.Map<IEnumerable<ClientCalendarEventDto>>(calendarEventProjections, opts =>
            {
                opts.Items["CurrentUserId"] = User.GetRequiredUserId();
            });

            return base.Ok(new ListResponse<ClientCalendarEventDto>(response));
        }
    }
}
