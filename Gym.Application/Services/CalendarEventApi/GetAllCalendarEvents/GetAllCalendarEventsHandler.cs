using Gym.Abstractions.Query.CalendarEvents;
using MediatR;

namespace Gym.Application.Services.CalendarEventApi.GetAllCalendarEvents
{
    internal class GetAllCalendarEventsHandler(ICalendarEventProjectionQueryService _calendarEventProjectionQueryService) 
        : IRequestHandler<GetAllCalendarEvents, IEnumerable<CalendarEventProjection>>
    {
        public async Task<IEnumerable<CalendarEventProjection>> Handle(GetAllCalendarEvents request, CancellationToken cancellationToken)
        {
            return await _calendarEventProjectionQueryService.GetAllAsync(cancellationToken);
        }
    }
}
