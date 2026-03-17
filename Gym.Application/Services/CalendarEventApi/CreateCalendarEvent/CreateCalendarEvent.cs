using Gym.Application.Aspects;
using Gym.Domain._Common;
using MediatR;

namespace Gym.Application.Services.CalendarEventApi.CreateCalendarEvent
{
    public record CreateCalendarEvent(
        DateTime Start,
        DateTime? End,
        Int32? MaxClientCount,
        String TrainingId,
        IEnumerable<String>? Instructors) : IRequest<Result<CreateCalendarEventResult>>, ITransactionalRequest;
}
