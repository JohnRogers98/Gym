using Gym.Domain._Shared;
using Gym.Domain.CalendarEventContext;
using MediatR;

namespace Gym.Application.Services.CalendarEventApi.GetCalendarEventById
{
    internal class GetCalendarEventByIdHandler(ICalendarEventRepository _calendarEventRepository) : IRequestHandler<GetCalendarEventById, CalendarEventDetails>
    {
        public async Task<CalendarEventDetails> Handle(GetCalendarEventById request, CancellationToken cancellationToken)
        {
            CalendarEvent? calendarEvent = await _calendarEventRepository.GetByIdAsync(CalendarEventId.From(request.Id), cancellationToken);

            if (calendarEvent == null) throw new ArgumentException();

            return calendarEvent.ToDetails();
        }
    }
}
