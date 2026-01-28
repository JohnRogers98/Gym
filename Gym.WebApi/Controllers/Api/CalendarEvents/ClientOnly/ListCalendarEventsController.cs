using AutoMapper;
using Gym.Application.Services.CalendarEventApi;
using Gym.Application.Services.CalendarEventApi.GetAllCalendarEvents;
using Gym.WebApi.Extensions;
using Gym.WebDto.Dto;
using Gym.WebDto.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.CalendarEvents.ClientOnly
{
    [Route("api/client-calendar-events")]
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.ClientOnly))]
    public class ListCalendarEventsController(IMediator _mediator, IMapper _mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<ListResponse<ClientCalendarEventDto>> ListClientCalendarEvents()
        {
            IEnumerable<CalendarEventDetails> calendarEventDetails = await _mediator.Send(new GetAllCalendarEventsQuery());
            return new (_mapper.Map<IEnumerable<ClientCalendarEventDto>>(calendarEventDetails, opts =>
            {
                opts.Items["CurrentUserId"] = User.GetRequiredUserId();
            }));
        }
    }
}
