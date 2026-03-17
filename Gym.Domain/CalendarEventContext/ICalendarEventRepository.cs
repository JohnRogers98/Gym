using Gym.Domain.CalendarEventContext.ValueObjects;

namespace Gym.Domain.CalendarEventContext
{
    public interface ICalendarEventRepository
    {
        CalendarEventId NextIdentity();
        Task SaveAsync(CalendarEvent calendarEvent, CancellationToken cancellationToken);
        Task<CalendarEvent?> GetByIdAsync(CalendarEventId id, CancellationToken cancellationToken);
        Task<Boolean> ExistsAsync(CalendarEventId id, CancellationToken cancellationToken);
    }
}
