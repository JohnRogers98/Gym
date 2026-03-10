using Gym.Domain.AccountContext;
using Gym.Domain.CalendarEventContext;

namespace Gym.Domain._Shared.Services
{
    public interface ICancelCalendarEventService
    {
        void Cancel(CalendarEvent calendarEvent, IReadOnlyCollection<Account> bookingAccounts);
    }

    public class CancelCalendarEventService : ICancelCalendarEventService
    {
        public void Cancel(CalendarEvent calendarEvent, IReadOnlyCollection<Account> bookingAccounts)
        {
            calendarEvent.Cancel();

            foreach (var account in bookingAccounts)
            {
                account.CancelBooking(calendarEvent.Id);
            }
        }
    }
}
