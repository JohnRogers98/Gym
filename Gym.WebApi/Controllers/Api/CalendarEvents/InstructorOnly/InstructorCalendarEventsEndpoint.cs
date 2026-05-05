using Ardalis.ApiEndpoints;
using AutoMapper;
using Gym.Abstractions.Query.CalendarEvents;
using Gym.Application.Services.CalendarEventApi.GetCalendarEventByInstructorId;
using Gym.WebApi.Extensions;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.CalendarEvent;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.CalendarEvents.InstructorOnly
{
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.InstructorOnly))]
    public class InstructorCalendarEventsEndpoint(IMediator _mediator, IMapper _mapper) : EndpointBaseAsync
        .WithoutRequest
        .WithActionResult<ListResponse<AdminCalendarEventDto>>
    {
        [HttpGet("api/instructors/me/admin-calendar-events")]
        public async override Task<ActionResult<ListResponse<AdminCalendarEventDto>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            IEnumerable<CalendarEventProjection> calendarEventProjections = await _mediator.Send(new GetCalendarEventByInstructorId(User.GetRequiredUserId()));

            var response = new ListResponse<AdminCalendarEventDto>(_mapper.Map<IEnumerable<AdminCalendarEventDto>>(calendarEventProjections));
            return base.Ok(response);
        }
    }
}
