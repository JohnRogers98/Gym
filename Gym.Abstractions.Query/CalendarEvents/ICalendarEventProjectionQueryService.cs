namespace Gym.Abstractions.Query.CalendarEvents
{
    public interface ICalendarEventProjectionQueryService
    {
        Task<CalendarEventProjection?> GetByIdAsync(String calendarEventId, CancellationToken cancellationToken);

        Task<IEnumerable<CalendarEventProjection>> GetAllAsync(CancellationToken cancellationToken);

        Task<IEnumerable<CalendarEventProjection>> GetStartingFromAsync(DateTime dateTime, CancellationToken cancellationToken);
    }
}
