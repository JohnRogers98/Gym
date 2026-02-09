using Gym.Domain.CalendarEventContext;
using MediatR;

namespace Gym.Application.Services.CalendarEventApi.GetAllCalendarEvents
{
    internal class GetAllCalendarEventsHandler(ICalendarEventQueryService _calendarEventQueryService) : IRequestHandler<GetAllCalendarEvents, IEnumerable<CalendarEventDetails>>
    {
        public async Task<IEnumerable<CalendarEventDetails>> Handle(GetAllCalendarEvents request, CancellationToken cancellationToken)
        {
            var calendarEvents = await _calendarEventQueryService.GetAllAsync(cancellationToken);
            return calendarEvents.Select(aCalendarEvent => aCalendarEvent.ToDetails());
        }
    }
}
