using AutoMapper;
using Gym.Abstractions.Query.CalendarEvents;
using Gym.Application.Services.CalendarEventApi.GetCalendarEventById;
using Gym.WebApi.Extensions;
using Gym.WebDto.Responses.CalendarEvent;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.CalendarEvents.AdminOnly
{
    [Route("api/admin-calendar-events/{id}")]
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.AdminOnly))]
    public class GetCalendarEventController(IMediator _mediator, IMapper _mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<GetAdminCalendarEventResponse> GetCalendarEvent(String id)
        {
            CalendarEventProjection calendarEventProjection = await _mediator.Send(new GetCalendarEventById(id));
            return _mapper.Map<GetAdminCalendarEventResponse>(calendarEventProjection);
        }
    }
}
