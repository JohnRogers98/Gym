using Gym.Abstractions.Query.CalendarEvents;
using MediatR;

namespace Gym.Application.Services.CalendarEventApi.GetCalendarEventById
{
    internal class GetCalendarEventByIdHandler(ICalendarEventProjectionQueryService calendarEventProjectionQueryService) 
        : IRequestHandler<GetCalendarEventById, CalendarEventProjection?>
    {
        public async Task<CalendarEventProjection?> Handle(GetCalendarEventById request, CancellationToken cancellationToken)
        {
            return await calendarEventProjectionQueryService.GetByIdAsync(request.Id, cancellationToken);
        }
    }
}
