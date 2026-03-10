using Gym.Abstractions.Query.CalendarEvents;
using MediatR;

namespace Gym.Application.Services.CalendarEventApi.GetCalendarEventById
{
    public record GetCalendarEventById(String Id) : IRequest<CalendarEventProjection>;
}
