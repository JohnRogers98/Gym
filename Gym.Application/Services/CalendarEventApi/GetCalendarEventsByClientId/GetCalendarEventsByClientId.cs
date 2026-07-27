using Gym.Abstractions.Query.CalendarEvents;
using MediatR;

namespace Gym.Application.Services.CalendarEventApi.GetCalendarEventsByClientId
{
    public record GetCalendarEventsByClientId(String ClientId) : IRequest<IEnumerable<CalendarEventProjection>>;
}
