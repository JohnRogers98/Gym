using MediatR;

namespace Gym.Application.Services.CalendarEventApi.GetCalendarEventById
{
    public record GetCalendarEventByIdQuery(String id) : IRequest<CalendarEventDetails>;
}
