using MediatR;

namespace Gym.Application.Services.CalendarEventApi.GetAllCalendarEvents
{
    public class GetAllCalendarEventsQuery : IRequest<IEnumerable<CalendarEventDetails>>;
}
