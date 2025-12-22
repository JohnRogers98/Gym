namespace Gym.Domain.CalendarEventAggregate
{
    public interface ICalendarEventQueryService
    {
        Task<CalendarEvent?> GetByIdAsync(CalendarEventId id, CancellationToken cancellationToken);
        Task<IEnumerable<CalendarEvent>> GetAllAsync(CancellationToken cancellationToken);
    }
}
