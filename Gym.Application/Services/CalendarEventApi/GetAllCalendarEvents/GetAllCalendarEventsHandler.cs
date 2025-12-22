using Gym.Application.Extensions;
using Gym.Domain.CalendarEventAggregate;
using MediatR;

namespace Gym.Application.Services.CalendarEventApi.GetAllCalendarEvents
{
    internal class GetAllCalendarEventsHandler(ICalendarEventQueryService _calendarEventQueryService) : IRequestHandler<GetAllCalendarEventsQuery, IEnumerable<CalendarEventDetails>>
    {
        public async Task<IEnumerable<CalendarEventDetails>> Handle(GetAllCalendarEventsQuery request, CancellationToken cancellationToken)
        {
            var calendarEvents = await _calendarEventQueryService.GetAllAsync(cancellationToken);
            return calendarEvents.Select(aCalendarEvent => aCalendarEvent.ToDetails());
        }
    }
}
