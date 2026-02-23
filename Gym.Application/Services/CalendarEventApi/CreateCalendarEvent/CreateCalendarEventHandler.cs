using Gym.Domain._Shared;
using Gym.Domain.CalendarEventContext;
using Gym.Domain.InstructorContext;
using Gym.Domain.TrainingContext;
using MediatR;

namespace Gym.Application.Services.CalendarEventApi.CreateCalendarEvent
{
    internal class CreateCalendarEventHandler(ICalendarEventRepository _calendarEventRepository) 
        : IRequestHandler<CreateCalendarEvent, CreateCalendarEventResult>
    {
        public async Task<CreateCalendarEventResult> Handle(CreateCalendarEvent request, CancellationToken cancellationToken)
        {
            CalendarEvent calendarEvent = CalendarEvent.Create(
                _calendarEventRepository.NextIdentity(),
                request.Start,
                request.End,
                TrainingId.From(request.TrainingId),
                new HashSet<UserId>(),
                request.MaxClientCount,
                request.Instructors.Select(InstructorId.From));

            await _calendarEventRepository.SaveAsync(calendarEvent, cancellationToken);

            return new CreateCalendarEventResult(calendarEvent.Id.Value);
        }
    }
}
