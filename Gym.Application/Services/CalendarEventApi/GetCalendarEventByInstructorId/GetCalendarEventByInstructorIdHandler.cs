using Gym.Abstractions.Query.CalendarEvents;
using MediatR;

namespace Gym.Application.Services.CalendarEventApi.GetCalendarEventByInstructorId
{
    internal class GetCalendarEventByInstructorIdHandler(ICalendarEventProjectionQueryService _calendarEventProjectionQueryService) 
        : IRequestHandler<GetCalendarEventByInstructorId, IEnumerable<CalendarEventProjection>>
    {
        public async Task<IEnumerable<CalendarEventProjection>> Handle(GetCalendarEventByInstructorId request, CancellationToken cancellationToken)
        {
            return await _calendarEventProjectionQueryService.GetAllByInstructorIdAsync(request.InstructorId, cancellationToken);
        }
    }
}
