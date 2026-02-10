using Gym.Domain._Shared;
using Gym.Domain.CalendarEventContext;
using MediatR;

namespace Gym.Application.Services.CalendarEventApi.CreateCalendarEvent
{
    internal class CreateCalendarEventHandler(ICalendarEventRepository _calendarEventRepository) : IRequestHandler<CreateCalendarEvent, CalendarEventDetails>
    {
        public async Task<CalendarEventDetails> Handle(CreateCalendarEvent request, CancellationToken cancellationToken)
        {
            CalendarEvent calendarEvent = CalendarEvent.Create(
                _calendarEventRepository.NextIdentity(),
                request.Start,
                request.End,
                request.Training.ToInfo(),
                new HashSet<UserId>(),
                request.MaxClientCount,
                request.Instructors.ToInfos());

            await _calendarEventRepository.SaveAsync(calendarEvent, cancellationToken);

            return calendarEvent.ToDetails();
        }
    }
}
