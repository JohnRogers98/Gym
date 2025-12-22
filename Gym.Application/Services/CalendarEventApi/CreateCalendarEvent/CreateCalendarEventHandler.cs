using Gym.Application.Extensions;
using Gym.Domain.CalendarEventAggregate;
using MediatR;

namespace Gym.Application.Services.CalendarEventApi.CreateCalendarEvent
{
    internal class CreateCalendarEventHandler(ICalendarEventRepository _calendarEventRepository) : IRequestHandler<CreateCalendarEventCommand, CalendarEventDetails>
    {
        public async Task<CalendarEventDetails> Handle(CreateCalendarEventCommand request, CancellationToken cancellationToken)
        {
            CalendarEvent calendarEvent = CalendarEvent.Create(
                _calendarEventRepository.NextIdentity(),
                request.start,
                request.end,
                request.training.ToInfo(),
                request.maxClientCount,
                request.instructors.ToInfos());

            await _calendarEventRepository.SaveAsync(calendarEvent, cancellationToken);

            return calendarEvent.ToDetails();
        }
    }
}
