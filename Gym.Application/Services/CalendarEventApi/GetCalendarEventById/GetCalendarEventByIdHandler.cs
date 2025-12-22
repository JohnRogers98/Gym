using Gym.Application.Extensions;
using Gym.Domain.CalendarEventAggregate;
using MediatR;

namespace Gym.Application.Services.CalendarEventApi.GetCalendarEventById
{
    internal class GetCalendarEventByIdHandler(ICalendarEventRepository _calendarEventRepository) : IRequestHandler<GetCalendarEventByIdQuery, CalendarEventDetails>
    {
        public async Task<CalendarEventDetails> Handle(GetCalendarEventByIdQuery request, CancellationToken cancellationToken)
        {
            CalendarEvent? calendarEvent = await _calendarEventRepository.GetByIdAsync(CalendarEventId.From(request.id), cancellationToken);

            if (calendarEvent == null) throw new ArgumentException();

            return calendarEvent.ToDetails();
        }
    }
}
