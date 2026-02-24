using Gym.Domain._Shared.Services;
using Gym.Domain.AccountContext;
using Gym.Domain.CalendarEventContext;

namespace Gym.Domain.Tests.Services
{
    public class CancelCalendarEventServiceTests
    {
        private FakeDataFixture _fakeDataFixture = new FakeDataFixture();

        [Fact]
        public void Service_Cancel_Calendar_Event_And_Bookings()
        {
            CancelCalendarEventService sut = new CancelCalendarEventService();
            Account account = _fakeDataFixture.CreateAccount(availableTrainingsCount: 1);
            CalendarEvent calendarEvent = _fakeDataFixture.CreateCalendarEvent();
            account.MakeBooking(calendarEvent.Id);
            calendarEvent.AddBooking(account.UserId);

            sut.Cancel(calendarEvent, new List<Account>() { account }.AsReadOnly());

            Assert.Equal(CalendarEventStatus.Cancelled, calendarEvent.Status);
            Assert.Equal(BookingStatus.Cancelled, account.Bookings.First(booking => booking.UserId == account.UserId).Status);
        }
    }
}
