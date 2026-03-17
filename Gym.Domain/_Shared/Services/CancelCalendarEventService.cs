using Gym.Application.Extensions;
using Gym.Domain._Common;
using Gym.Domain.AccountContext;
using Gym.Domain.CalendarEventContext;

namespace Gym.Domain._Shared.Services
{
    public interface ICancelCalendarEventService
    {
        Result Cancel(CalendarEvent calendarEvent, IReadOnlyCollection<Account> bookingAccounts);
    }

    public class CancelCalendarEventService : ICancelCalendarEventService
    {
        public Result Cancel(CalendarEvent calendarEvent, IReadOnlyCollection<Account> bookingAccounts)
        {
            return calendarEvent.Cancel()
              .Bind(() =>
              {
                  foreach (var account in bookingAccounts)
                  {
                      Result completeBookingResult = account.CancelBooking(calendarEvent.Id);
                      if (completeBookingResult.Success is false)
                      {
                          return completeBookingResult;
                      }
                  }
                  return Result.Ok();
              });
        }
    }
}
