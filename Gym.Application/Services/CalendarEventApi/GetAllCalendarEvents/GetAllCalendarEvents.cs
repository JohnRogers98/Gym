using MediatR;

namespace Gym.Application.Services.CalendarEventApi.GetAllCalendarEvents
{
    public class GetAllCalendarEvents : IRequest<IEnumerable<CalendarEventDetails>>;
}
