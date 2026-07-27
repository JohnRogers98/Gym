using Gym.Abstractions.Query.CalendarEvents;
using MediatR;

namespace Gym.Application.Services.CalendarEventApi.GetCalendarEventByInstructorId
{
    public record GetCalendarEventByInstructorId(String InstructorId) : IRequest<IEnumerable<CalendarEventProjection>>;
}
