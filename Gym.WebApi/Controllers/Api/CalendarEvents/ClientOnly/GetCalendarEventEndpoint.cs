using Ardalis.ApiEndpoints;
using AutoMapper;
using Gym.Abstractions.Query.CalendarEvents;
using Gym.Application.Services.CalendarEventApi.GetCalendarEventById;
using Gym.WebApi.Extensions;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.CalendarEvent;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static Gym.WebApi.Controllers.Api.CalendarEvents.ClientOnly.GetCalendarEventEndpoint;

namespace Gym.WebApi.Controllers.Api.CalendarEvents.ClientOnly
{
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.Client))]
    public class GetCalendarEventEndpoint(IMediator _mediator, IMapper _mapper) : EndpointBaseAsync
        .WithRequest<GetCalendarEventByIdContainer>
        .WithActionResult<Response<ClientCalendarEventDto>>
    {
        [HttpGet("api/client-calendar-events/{id}")]
        public override async Task<ActionResult<Response<ClientCalendarEventDto>>> HandleAsync(GetCalendarEventByIdContainer request, CancellationToken cancellationToken = default)
        {
            CalendarEventProjection? calendarEventProjection = await _mediator.Send(new GetCalendarEventById(request.Id), cancellationToken);
            
            if(calendarEventProjection is null)
            {
                return base.NotFound($"Calendar event by id - {request.Id} not found.");
            }

            var response = new Response<ClientCalendarEventDto>(_mapper.Map<ClientCalendarEventDto>(calendarEventProjection, opts =>
            {
                opts.Items["CurrentUserId"] = User.GetRequiredUserId();
            }));

            return base.Ok(response);
        }

        public class GetCalendarEventByIdContainer
        {
            [FromRoute] public String Id { get; set; } = default!;
        }

    }
}
