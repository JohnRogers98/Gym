using Gym.Application.Extensions;
using Gym.Domain.AccountContext.Entities;
using Gym.Domain.AccountContext.Errors;
using Gym.Domain.AccountContext.ValueObjects;

namespace Gym.Domain.Tests.AccountContext
{    
    public class BookingTests
    {
        private readonly FakeDataFixture _fakeDataFixture = new();

        [Fact]
        public void Check_Correct_State_After_Booking_Create()
        {
            Booking sut = _fakeDataFixture.GenerateBooking();

            Assert.Equal(BookingStatus.Upcoming, sut.Status);
        }

        [Fact]
        public void Check_Correct_State_After_Booking_Cancelled()
        {
            Booking sut = _fakeDataFixture.GenerateBooking();

            sut.Cancel();

            Assert.Equal(BookingStatus.Cancelled, sut.Status);
        }

        [Fact]
        public void Check_Correct_State_After_Rebooked()
        {
            Booking sut = _fakeDataFixture.GenerateBooking();
            
            sut.Cancel()
                .Bind(sut.Rebook);

            Assert.Equal(BookingStatus.Upcoming, sut.Status);
        }

        [Fact]
        public void Check_Correct_State_After_Marking_As_Completed()
        {
            Booking sut = _fakeDataFixture.GenerateBooking();

            sut.MarkAsCompleted();

            Assert.Equal(BookingStatus.Completed, sut.Status);
        }

        [Fact]
        public void Check_Returns_Error_When_Cancelling_Again()
        {
            Booking sut = _fakeDataFixture.GenerateBooking();

            var result = sut.Cancel()
                .Bind(sut.Cancel);

            Assert.IsType<IncorrectBookingStatusStateError>(result.Error);
        }

        [Fact]
        public void Check_Returns_Error_When_Rebooked_Upcoming()
        {
            Booking sut = _fakeDataFixture.GenerateBooking();

            var result = sut.Rebook();
            Assert.IsType<IncorrectBookingStatusStateError>(result.Error);
        }

        [Fact]
        public void Check_Return_Error_When_Completed_Again()
        {
            Booking sut = _fakeDataFixture.GenerateBooking();

            var result = sut.MarkAsCompleted()
                .Bind(sut.MarkAsCompleted);

            Assert.IsType<IncorrectBookingStatusStateError>(result.Error);
        }

        [Fact]
        public void Check_Returns_Error_When_Cancelling_After_Completion()
        {
            Booking sut = _fakeDataFixture.GenerateBooking();

            var result = sut.MarkAsCompleted()
                .Bind(sut.Cancel);

            Assert.IsType<IncorrectBookingStatusStateError>(result.Error);
        }

        [Fact]
        public void Check_Returns_Error_When_Rebooked_After_Completion()
        {
            Booking sut = _fakeDataFixture.GenerateBooking();

            var result = sut.MarkAsCompleted()
                .Bind(sut.Rebook);

            Assert.IsType<IncorrectBookingStatusStateError>(result.Error);
        }
    }
}
