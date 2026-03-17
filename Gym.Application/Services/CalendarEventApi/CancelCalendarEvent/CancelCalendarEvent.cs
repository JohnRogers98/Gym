using Gym.Application.Aspects;
using Gym.Domain._Common;
using MediatR;

namespace Gym.Application.Services.CalendarEventApi.CancelCalendarEvent
{
    public record class CancelCalendarEvent(String CalendarEventId) : IRequest<Result<CancelCalendarEventResult>>, ITransactionalRequest;
}
