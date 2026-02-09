using Gym.Domain.AccountContext;

namespace Gym.Application.Services.BookingApi
{
    internal static class BookingExtensions
    {
        public static BookingDetails ToDetails(this Booking booking) => new BookingDetails(booking.Id.Value);
    }
}
