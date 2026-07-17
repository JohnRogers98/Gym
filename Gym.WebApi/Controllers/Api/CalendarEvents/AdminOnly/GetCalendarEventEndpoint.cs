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
using static Gym.WebApi.Controllers.Api.CalendarEvents.AdminOnly.GetCalendarEventEndpoint;

namespace Gym.WebApi.Controllers.Api.CalendarEvents.AdminOnly
{
    [Route("api/admin-calendar-events/{id}")]
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.Admin))]
    public class GetCalendarEventEndpoint(IMediator _mediator, IMapper _mapper) : EndpointBaseAsync
        .WithRequest<GetCalendarEventByIdContainer>
        .WithActionResult<Response<AdminCalendarEventDto>>
    {
        [HttpGet]
        public override async Task<ActionResult<Response<AdminCalendarEventDto>>> HandleAsync(GetCalendarEventByIdContainer request, CancellationToken cancellationToken = default)
        {
            CalendarEventProjection? calendarEventProjection = await _mediator.Send(new GetCalendarEventById(request.Id), cancellationToken);

            if(calendarEventProjection is null)
            {
                return base.NotFound($"Calendar event with id {request.Id} -  not found.");
            }

            var response = new Response<AdminCalendarEventDto>(_mapper.Map<AdminCalendarEventDto>(calendarEventProjection));
            return base.Ok(response);
        }

        public class GetCalendarEventByIdContainer
        {
            [FromRoute] public String Id { get; set; } = default!;
        }
    }
}
