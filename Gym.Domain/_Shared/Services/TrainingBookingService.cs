using Gym.Domain._Common;
using Gym.Domain.BookingAggregate;
using Gym.Domain.CalendarEventAggregate;
using Gym.Domain.CalendarEventAggregate.Errors;
using Gym.Domain.ClientAggregate;

namespace Gym.Domain._Shared.Services
{
    public interface ITrainingBookingService
    {
        Result<Booking> MakeEventBooking(CalendarEvent calendarEvent, Client client);
    }

    public class TrainingBookingService(IBookingRepository _bookingRepository) : ITrainingBookingService
    {
        public Result<Booking> MakeEventBooking(CalendarEvent calendarEvent, Client client)
        {
            Result possibilityValidationResult = this.ValidateBookingPossibility(calendarEvent, client);
            if (possibilityValidationResult.Success is false)
            {
                return Result<Booking>.Fail(possibilityValidationResult.Error!);
            }

            Booking booking = Booking.Create(_bookingRepository.NextIdentity(), client.UserId, calendarEvent.Id);
            
            if(calendarEvent.HasExpired(booking.ChangedAt) is true)
            {
                return Result<Booking>.Fail(EventTimeHasExpired.Create(calendarEvent.Id));
            }

            calendarEvent.AddBooking(booking.UserId);

            return Result<Booking>.Ok(booking);
        }

        private Result ValidateBookingPossibility(CalendarEvent calendarEvent, Client client)
        {
            if (calendarEvent.HasBookingFor(client.UserId))
            {
                return Result.Fail(UserAlreadyBookedError.Create(calendarEvent.Id, client.UserId));
            }
            if (calendarEvent.HasFreeSpace() is false)
            {
                return Result.Fail(EventHasNotFreeSpaceError.Create(calendarEvent.Id));
            }

            return Result.Ok();
        }
    }
}
