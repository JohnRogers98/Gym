namespace Gym.Domain.BookingAggregate
{
    public interface IBookingRepository
    {
        BookingId NextIdentity();
        Task SaveAsync(Booking booking, CancellationToken cancellationToken);
        Task<Booking?> GetByIdAsync(BookingId id, CancellationToken cancellationToken);
        Task<Boolean> ExistsAsync(BookingId id, CancellationToken cancellationToken);
    }
}
