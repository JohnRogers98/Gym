using Gym.Domain._Common;

namespace Gym.Domain.BookingAggregate.Errors
{
    public class IncorrectBookingStatusStateError : DomainError
    {
        public BookingId BookingId { get; }
        public BookingStatus Status { get; }

        private IncorrectBookingStatusStateError(BookingId bookingId, BookingStatus status) : base(nameof(IncorrectBookingStatusStateError))
        {
            BookingId = bookingId;
            Status = status;
        }

        public static IncorrectBookingStatusStateError Create(BookingId bookingId, BookingStatus status) => new(bookingId, status);

        public override String GetErrorMessage() => $"Current booking - {BookingId} status - {Status} incorrect for operation.";
    }
}
