using AutoMapper;
using Gym.Application.Services.CalendarEventApi;
using Gym.Application.Services.CalendarEventApi.GetAllCalendarEvents;
using Gym.WebDto.Dto;
using Gym.WebDto.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.CalendarEvents
{
    [Route("api/calendar-events")]
    [ApiController]
    public class ListCalendarEventsController(IMediator _mediator, IMapper _mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<ListResponse<CalendarEventDto>> ListCalendarEvents()
        {
            IEnumerable<CalendarEventDetails> calendarEventDetails = await _mediator.Send(new GetAllCalendarEventsQuery());
            return new (_mapper.Map<IEnumerable<CalendarEventDto>>(calendarEventDetails));
        }
    }
}
