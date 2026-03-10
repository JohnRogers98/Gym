using Gym.Domain.AccountContext;
using Gym.Domain.CalendarEventContext;

namespace Gym.Domain._Shared.Services
{
    public interface ICompleteCalendarEventService
    {
        void Complete(CalendarEvent calendarEvent, IReadOnlyCollection<Account> bookingAccounts);
    }

    public class CompleteCalendarEventService : ICompleteCalendarEventService
    {
        public void Complete(CalendarEvent calendarEvent, IReadOnlyCollection<Account> bookingAccounts)
        {
            calendarEvent.Complete();

            foreach (var account in bookingAccounts)
            {
                account.CompleteBooking(calendarEvent.Id);
            }
        }
    }
}
