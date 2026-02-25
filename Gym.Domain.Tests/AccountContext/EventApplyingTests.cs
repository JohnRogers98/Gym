using Gym.Domain.AccountContext;
using Gym.Domain.AccountContext.Events;

namespace Gym.Domain.Tests.AccountContext
{
    public class EventApplyingTests
    {
        private readonly FakeDataFixture _fakeDataFixture = new();

        #region Apply TrainingBookedDomainEvent
        [Theory]
        [InlineData(1, 0)]
        [InlineData(2, 1)]
        public void Apply_Training_Booked_Subtract_Available_Training_Count(Int32 availableCount, Int32 expectedCountAfterApplying)
        {
            var trainingBookedDomainEvent = TrainingBookedDomainEvent.Create(_fakeDataFixture.BookingId, _fakeDataFixture.UserId, _fakeDataFixture.CalendarEventId);
            Account sut = _fakeDataFixture.CreateAccount(availableTrainingsCount: availableCount);

            sut.ApplyEvent(trainingBookedDomainEvent);

            Assert.Equal(expectedCountAfterApplying, sut.AvailableTrainingsCount);
            Assert.Single(sut.Bookings);
        }
        #endregion

        #region Apply TrainingBookingCancelledDomainEvent
        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        public void Apply_Training_Booking_Cancelled_Add_Available_Training_Count(Int32 availableCount, Int32 expectedCountAfterApplying)
        {
            var trainingBookingCancelledDomainEvent = TrainingCancelledDomainEvent.Create(_fakeDataFixture.BookingId, _fakeDataFixture.UserId, _fakeDataFixture.CalendarEventId);
            Account sut = _fakeDataFixture.CreateAccount(availableTrainingsCount: availableCount);
            sut.MakeBooking(_fakeDataFixture.CalendarEventId);

            sut.ApplyEvent(trainingBookingCancelledDomainEvent);

            Assert.Equal(expectedCountAfterApplying, sut.AvailableTrainingsCount);
        }
        #endregion

        #region Apply TrainingRebookedDomainEvent

        [Theory]
        [InlineData(1, 0)]
        [InlineData(2, 1)]
        public void Apply_Training_Rebooked_Subtract_Available_Training_Count(Int32 availableCount, Int32 expectedCountAfterApplying)
        {
            var trainingRebookedDomainEvent = TrainingRebookedDomainEvent.Create(_fakeDataFixture.BookingId, _fakeDataFixture.UserId, _fakeDataFixture.CalendarEventId);
            Account sut = _fakeDataFixture.CreateAccount(availableTrainingsCount: availableCount);
            sut.MakeBooking(_fakeDataFixture.CalendarEventId);
            sut.CancelBooking(_fakeDataFixture.CalendarEventId);

            sut.ApplyEvent(trainingRebookedDomainEvent);

            Assert.Equal(expectedCountAfterApplying, sut.AvailableTrainingsCount);
        }
        #endregion

        #region AccountChargedDomainEvent
        [Theory]
        [InlineData(0, 5, 5)]
        [InlineData(2, 3, 5)]
        public void Apply_Account_Charged_Add_Available_Training_Count(Int32 availableCount, Int32 chargeCount, Int32 expectedCountAfterApplying)
        {
            var accountChargedDomainEvent = AccountChargedDomainEvent.Create(_fakeDataFixture.UserId, chargeCount);
            Account sut = _fakeDataFixture.CreateAccount(availableTrainingsCount: availableCount);

            sut.ApplyEvent(accountChargedDomainEvent);

            Assert.Equal(expectedCountAfterApplying, sut.AvailableTrainingsCount);
        }
        #endregion

       
    }
}
