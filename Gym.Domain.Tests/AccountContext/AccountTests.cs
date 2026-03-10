using Gym.Domain._Exceptions;
using Gym.Domain.AccountContext;
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
            Assert.Equal(0, sut.AvailableTrainingsCount);
            Assert.NotEqual(default, sut.DomainEvents.OfType<TrainingBookedDomainEvent>().SingleOrDefault());
        }

        [Fact]
        public void Check_Correct_State_After_Booking_Cancelled()
        {
            Account sut = _fakeDataFixture.CreateAccount(availableTrainingsCount: 1);
            sut.MakeBooking(_fakeDataFixture.CalendarEventId);
            sut.ClearDomainEvents();

            sut.CancelBooking(_fakeDataFixture.CalendarEventId);

            Assert.Equal(1, sut.AvailableTrainingsCount);
            Assert.NotEqual(default, sut.DomainEvents.OfType<TrainingCancelledDomainEvent>().SingleOrDefault());
        }

        [Fact]
        public void Check_Correct_State_After_Rebooked()
        {
            Account sut = _fakeDataFixture.CreateAccount(availableTrainingsCount: 1);
            sut.MakeBooking(_fakeDataFixture.CalendarEventId);
            sut.CancelBooking(_fakeDataFixture.CalendarEventId);
            sut.ClearDomainEvents();

            sut.Rebook(_fakeDataFixture.CalendarEventId);

            Assert.Equal(0, sut.AvailableTrainingsCount);
            Assert.NotEqual(default, sut.DomainEvents.OfType<TrainingRebookedDomainEvent>().SingleOrDefault());
        }

        [Fact]
        public void Check_Correct_State_After_Marking_As_Completed()
        {
            Account sut = _fakeDataFixture.CreateAccount(availableTrainingsCount: 1);
            sut.MakeBooking(_fakeDataFixture.CalendarEventId);
            sut.ClearDomainEvents();

            sut.CompleteBooking(_fakeDataFixture.CalendarEventId);

            Assert.NotEqual(default, sut.DomainEvents.OfType<TrainingCompletedDomainEvent>().SingleOrDefault());
        }

        [Fact]
        public void Check_Throws_Domain_Exception_When_Cancelling_Again()
        {
            Account sut = _fakeDataFixture.CreateAccount(availableTrainingsCount: 1);
            sut.MakeBooking(_fakeDataFixture.CalendarEventId);
            sut.CancelBooking(_fakeDataFixture.CalendarEventId);

            Assert.Throws<DomainException>(() => sut.CancelBooking(_fakeDataFixture.CalendarEventId));
        }

        [Fact]
        public void Check_Throws_Domain_Exception_When_Rebooked_Again()
        {
            Account sut = _fakeDataFixture.CreateAccount(availableTrainingsCount: 1);
            sut.MakeBooking(_fakeDataFixture.CalendarEventId);

            Assert.Throws<DomainException>(() => sut.Rebook(_fakeDataFixture.CalendarEventId));
        }

        [Fact]
        public void Check_Throws_Domain_Exception_When_Completed_Again()
        {
            Account sut = _fakeDataFixture.CreateAccount(availableTrainingsCount: 1);
            sut.MakeBooking(_fakeDataFixture.CalendarEventId);

            sut.CompleteBooking(_fakeDataFixture.CalendarEventId);

            Assert.Throws<DomainException>(() => sut.CompleteBooking(_fakeDataFixture.CalendarEventId));
        }

        [Fact]
        public void Check_Throws_Domain_Exception_When_Cancelling_After_Completion()
        {
            Account sut = _fakeDataFixture.CreateAccount(availableTrainingsCount: 1);
            sut.MakeBooking(_fakeDataFixture.CalendarEventId);
            sut.CompleteBooking(_fakeDataFixture.CalendarEventId);

            Assert.Throws<DomainException>(() => sut.CancelBooking(_fakeDataFixture.CalendarEventId));
        }

        [Fact]
        public void Check_Throws_Domain_Exception_When_Rebooked_After_Completion()
        {
            Account sut = _fakeDataFixture.CreateAccount(availableTrainingsCount: 1);
            sut.MakeBooking(_fakeDataFixture.CalendarEventId);
            sut.CompleteBooking(_fakeDataFixture.CalendarEventId);

            Assert.Throws<DomainException>(() => sut.Rebook(_fakeDataFixture.CalendarEventId));
        }
    }
}
