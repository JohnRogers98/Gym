using Gym.Application.Aspects;
using MediatR;

namespace Gym.Application.Services.CalendarEventApi.CancelCalendarEvent
{
    public record class CancelCalendarEvent(String CalendarEventId) : IRequest<CancelCalendarEventResult>, ITransactionalRequest;
}
