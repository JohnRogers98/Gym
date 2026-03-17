using Gym.Application.Extensions;
using Gym.Domain.AccountContext;
using Gym.Domain.AccountContext.Errors;
using Gym.Domain.AccountContext.Events;

namespace Gym.Domain.Tests.AccountContext
{
    public class AccountTests
    {
        private readonly FakeDataFixture _fakeDataFixture = new();

        [Fact]
        public void Check_Correct_State_After_Making_Book()
        {
            Account sut = _fakeDataFixture.CreateAccount(availableTrainingsCount: 1);

            sut.MakeBooking(_fakeDataFixture.CalendarEventId);

            Assert.Single(sut.Bookings);
            Assert.Equal(0, sut.RemainingTrainings.Value);
            Assert.NotEqual(default, sut.DomainEvents.OfType<TrainingBookedDomainEvent>().SingleOrDefault());
        }

        [Fact]
        public void Check_Correct_State_After_Booking_Cancelled()
        {
            Account sut = _fakeDataFixture.CreateAccount(availableTrainingsCount: 1);

            sut.MakeBooking(_fakeDataFixture.CalendarEventId)
                .Bind(() => sut.CancelBooking(_fakeDataFixture.CalendarEventId));

            Assert.Equal(1, sut.RemainingTrainings.Value);
            Assert.NotEqual(default, sut.DomainEvents.OfType<TrainingCancelledDomainEvent>().SingleOrDefault());
        }

        [Fact]
        public void Check_Correct_State_After_Rebooked()
        {
            Account sut = _fakeDataFixture.CreateAccount(availableTrainingsCount: 1);

            sut.MakeBooking(_fakeDataFixture.CalendarEventId)
                .Bind(() => sut.CancelBooking(_fakeDataFixture.CalendarEventId))
                .Bind(() => sut.Rebook(_fakeDataFixture.CalendarEventId));

            Assert.Equal(0, sut.RemainingTrainings.Value);
            Assert.NotEqual(default, sut.DomainEvents.OfType<TrainingRebookedDomainEvent>().SingleOrDefault());
        }

        [Fact]
        public void Check_Correct_State_After_Marking_As_Completed()
        {
            Account sut = _fakeDataFixture.CreateAccount(availableTrainingsCount: 1);
           
            sut.MakeBooking(_fakeDataFixture.CalendarEventId)
                .Bind(() => sut.CompleteBooking(_fakeDataFixture.CalendarEventId));

            Assert.NotEqual(default, sut.DomainEvents.OfType<TrainingCompletedDomainEvent>().SingleOrDefault());
        }

        [Fact]
        public void Check_Returns_Error_When_Cancelling_Again()
        {
            Account sut = _fakeDataFixture.CreateAccount(availableTrainingsCount: 1);

            var result = sut.MakeBooking(_fakeDataFixture.CalendarEventId)
                .Bind(() => sut.CancelBooking(_fakeDataFixture.CalendarEventId))
                .Bind(() => sut.CancelBooking(_fakeDataFixture.CalendarEventId));

            Assert.IsType<IncorrectBookingStatusStateError>(result.Error);
        }

        [Fact]
        public void Check_Returns_Error_When_Rebooked_Again()
        {
            Account sut = _fakeDataFixture.CreateAccount(availableTrainingsCount: 1);

            var result = sut.MakeBooking(_fakeDataFixture.CalendarEventId)
                .Bind(() => sut.Rebook(_fakeDataFixture.CalendarEventId));

            Assert.IsType<IncorrectBookingStatusStateError>(result.Error);
        }

        [Fact]
        public void Check_Returns_Error_When_Completed_Again()
        {
            Account sut = _fakeDataFixture.CreateAccount(availableTrainingsCount: 1);

            var result = sut.MakeBooking(_fakeDataFixture.CalendarEventId)
                .Bind(() => sut.CompleteBooking(_fakeDataFixture.CalendarEventId))
                .Bind(() => sut.CompleteBooking(_fakeDataFixture.CalendarEventId));

            Assert.IsType<IncorrectBookingStatusStateError>(result.Error);
        }

        [Fact]
        public void Check_Returns_Error_When_Cancelling_After_Completion()
        {
            Account sut = _fakeDataFixture.CreateAccount(availableTrainingsCount: 1);

            var result = sut.MakeBooking(_fakeDataFixture.CalendarEventId)
                .Bind(() => sut.CompleteBooking(_fakeDataFixture.CalendarEventId))
                .Bind(() => sut.CancelBooking(_fakeDataFixture.CalendarEventId));

            Assert.IsType<IncorrectBookingStatusStateError>(result.Error);
        }

        [Fact]
        public void Check_Returns_Error_When_Rebooked_After_Completion()
        {
            Account sut = _fakeDataFixture.CreateAccount(availableTrainingsCount: 1);

            var result = sut.MakeBooking(_fakeDataFixture.CalendarEventId)
                .Bind(() => sut.CompleteBooking(_fakeDataFixture.CalendarEventId))
                .Bind(() => sut.Rebook(_fakeDataFixture.CalendarEventId));

            Assert.IsType<IncorrectBookingStatusStateError>(result.Error);
        }
    }
}
