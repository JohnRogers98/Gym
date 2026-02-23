using Gym.Domain._Exceptions;
using Gym.Domain._Shared;
using Gym.Domain.AccountContext;

namespace Gym.Domain.Tests.AccountContext
{    
    public class BookingTests
    {
        [Fact]
        public void Check_Correct_State_After_Booking_Create()
        {
            Booking booking = this.CreateBooking();

            Assert.Equal(BookingStatus.Upcoming, booking.Status);
        }

        [Fact]
        public void Check_Correct_State_After_Booking_Cancelled()
        {
            Booking booking = this.CreateBooking();

            booking.Cancel();

            Assert.Equal(BookingStatus.Cancelled, booking.Status);
        }

        [Fact]
        public void Check_Correct_State_After_Rebooked()
        {
            var booking = CreateBooking();
            booking.Cancel();

            booking.Rebook();

            Assert.Equal(BookingStatus.Upcoming, booking.Status);
        }

        [Fact]
        public void Check_Correct_State_After_Marking_As_Completed()
        {
            Booking booking = this.CreateBooking();

            booking.MarkAsCompleted();

            Assert.Equal(BookingStatus.Completed, booking.Status);
        }

        [Fact]
        public void Check_Throws_Domain_Exception_When_Cancelling_Again()
        {
            Booking booking = this.CreateBooking();

            booking.Cancel();

            Assert.Throws<DomainException>(booking.Cancel);
        }

        [Fact]
        public void Check_Throws_Domain_Exception_When_Rebooked_Again()
        {
            Booking booking = this.CreateBooking();

            Assert.Throws<DomainException>(booking.Rebook);
        }

        [Fact]
        public void Check_Throws_Domain_Exception_When_Completed_Again()
        {
            Booking booking = this.CreateBooking();

            booking.MarkAsCompleted();

            Assert.Throws<DomainException>(booking.MarkAsCompleted);
        }

        [Fact]
        public void Check_Throws_Domain_Exception_When_Cancelling_After_Completion()
        {
            Booking booking = this.CreateBooking();

            booking.MarkAsCompleted();

            Assert.Throws<DomainException>(booking.Cancel);
        }

        [Fact]
        public void Check_Throws_Domain_Exception_When_Rebooked_After_Completion()
        {
            Booking booking = this.CreateBooking();

            booking.MarkAsCompleted();

            Assert.Throws<DomainException>(booking.Rebook);
        }


        private Booking CreateBooking()
        {
            return Booking.Create(
                BookingId.From(Guid.NewGuid().ToString()),
                UserId.From(Guid.NewGuid().ToString()),
                CalendarEventId.From(Guid.NewGuid().ToString()));
        }
    }
}
