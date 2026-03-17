using Gym.Application.Extensions;
using Gym.Domain._Common;
using Gym.Domain.AccountContext;
using Gym.Domain.AccountContext.Entities;
using Gym.Domain.CalendarEventContext;

namespace Gym.Domain._Shared.Services
{
    public interface ITrainingBookingService
    {
        Result<Booking> MakeEventBooking(Account account, CalendarEvent calendarEvent);
    }

    public class TrainingBookingService : ITrainingBookingService
    {
        public Result<Booking> MakeEventBooking(Account account, CalendarEvent calendarEvent)
        {
            return calendarEvent.AddBooking(account.UserId)
                .Bind(() => account.MakeBooking(calendarEvent.Id));
        }
    }
}
