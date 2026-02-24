namespace Gym.Domain.CalendarEventContext
{
    public interface IPastCalendarEventsFinder
    {
        Task<IEnumerable<CalendarEvent>> GetPastCalendarEventsAsync(DateTime currentDateTime, CancellationToken cancellationToken);
    }
}
