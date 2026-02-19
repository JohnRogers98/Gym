using AutoMapper;
using Gym.Application.Services.CalendarEventApi;
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
        public async Task<ListResponse<AdminCalendarEventDto>> ListCalendarEvents()
        {
            IEnumerable<CalendarEventDetails> calendarEventDetails = await _mediator.Send(new GetAllCalendarEvents());
            return new (_mapper.Map<IEnumerable<AdminCalendarEventDto>>(calendarEventDetails));
        }
    }
}
