using AutoMapper;
using Gym.Application.Services.CalendarEventApi;
using Gym.Application.Services.CalendarEventApi.GetCalendarEventById;
using Gym.WebDto.Responses.CalendarEvent;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.CalendarEvents
{
    [Route("api/calendar-events")]
    [ApiController]
    public class GetCalendarEventController(IMediator _mediator, IMapper _mapper) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<GetCalendarEventResponse> GetCalendarEvent(String id)
        {
            CalendarEventDetails calendarEventDetails = await _mediator.Send(_mapper.Map<GetCalendarEventByIdQuery>(id));
            return _mapper.Map<GetCalendarEventResponse>(calendarEventDetails);
        }
    }
}
