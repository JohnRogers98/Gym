using Gym.Domain._Shared.Services;
using Gym.Domain.AccountContext;
using Gym.Domain.CalendarEventContext;

namespace Gym.Domain.Tests.Services
{ 
    public class TrainingBookingServiceTests
    {
        private FakeDataFixture _fakeDataFixture = new FakeDataFixture();

        [Fact]
        public void Service_Dispatсh_Booking_Between_Account_And_Calendar_Event()
        {
            TrainingBookingService sut = new TrainingBookingService();
            Account account = _fakeDataFixture.CreateAccount(availableTrainingsCount: 1);
            CalendarEvent calendarEvent = _fakeDataFixture.CreateCalendarEvent();

            sut.MakeEventBooking(account, calendarEvent);

            Assert.Equal(0, account.AvailableTrainingsCount);
            Assert.Single(account.Bookings);
            Assert.Single(calendarEvent.Bookings);
            Assert.True(calendarEvent.HasBookingFor(_fakeDataFixture.UserId));
        }
    }
}
