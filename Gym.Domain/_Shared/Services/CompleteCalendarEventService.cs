using Gym.Application.Extensions;
using Gym.Domain._Common;
using Gym.Domain.AccountContext;
using Gym.Domain.CalendarEventContext;

namespace Gym.Domain._Shared.Services
{
    public interface ICompleteCalendarEventService
    {
        Result Complete(CalendarEvent calendarEvent, IReadOnlyCollection<Account> bookingAccounts);
    }

    public class CompleteCalendarEventService : ICompleteCalendarEventService
    {
        public Result Complete(CalendarEvent calendarEvent, IReadOnlyCollection<Account> bookingAccounts)
        {
            return calendarEvent.Complete()
                .Bind(() =>
                {
                    foreach (var account in bookingAccounts)
                    {
                        Result completeBookingResult = account.CompleteBooking(calendarEvent.Id);
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
