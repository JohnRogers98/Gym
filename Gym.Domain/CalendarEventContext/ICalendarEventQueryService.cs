using Gym.Domain._Shared;

namespace Gym.Domain.CalendarEventContext
{
    public interface ICalendarEventQueryService
    {
        Task<CalendarEvent?> GetByIdAsync(CalendarEventId id, CancellationToken cancellationToken);
        Task<IEnumerable<CalendarEvent>> GetAllAsync(CancellationToken cancellationToken);
        Task<Boolean> ExistsAsync(CalendarEventId id, CancellationToken cancellationToken);
    }
}
