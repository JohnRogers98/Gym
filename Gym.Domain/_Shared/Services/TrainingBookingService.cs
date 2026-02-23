using Gym.Domain.AccountContext;
using Gym.Domain.CalendarEventContext;

namespace Gym.Domain._Shared.Services
{
    public interface ITrainingBookingService
    {
        Booking MakeEventBooking(Account account, CalendarEvent calendarEvent);
    }

    public class TrainingBookingService : ITrainingBookingService
    {
        public Booking MakeEventBooking(Account account, CalendarEvent calendarEvent)
        {
            calendarEvent.AddBooking(account.UserId);
            
            return account.MakeBooking(calendarEvent.Id);
        }
    }
}
