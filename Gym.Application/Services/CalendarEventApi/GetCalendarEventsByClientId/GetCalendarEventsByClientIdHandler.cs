using Gym.Abstractions.Query.CalendarEvents;
using MediatR;

namespace Gym.Application.Services.CalendarEventApi.GetCalendarEventsByClientId
{
    internal class GetCalendarEventsByClientIdHandler(ICalendarEventProjectionQueryService _calendarEventProjectionQueryService) 
        : IRequestHandler<GetCalendarEventsByClientId, IEnumerable<CalendarEventProjection>>
    {
        public async Task<IEnumerable<CalendarEventProjection>> Handle(GetCalendarEventsByClientId request, CancellationToken cancellationToken)
        {
            return await _calendarEventProjectionQueryService.GetAllByClientIdAsync(request.ClientId, cancellationToken);
        }
    }
}
