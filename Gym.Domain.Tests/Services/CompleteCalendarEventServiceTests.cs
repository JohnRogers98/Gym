using Gym.Domain._Shared.Services;
using Gym.Domain.AccountContext;
using Gym.Domain.AccountContext.ValueObjects;
using Gym.Domain.CalendarEventContext;
using Gym.Domain.CalendarEventContext.ValueObjects;

namespace Gym.Domain.Tests.Services
{
    public class CompleteCalendarEventServiceTests
    {
        private FakeDataFixture _fakeDataFixture = new FakeDataFixture();

        [Fact]
        public void Service_Complete_Calendar_Event_And_Bookings()
        {
            CompleteCalendarEventService sut = new CompleteCalendarEventService();
            Account account = _fakeDataFixture.CreateAccount(availableTrainingsCount: 1);
            CalendarEvent calendarEvent = _fakeDataFixture.CreateCalendarEvent();
            account.MakeBooking(calendarEvent.Id);
            calendarEvent.AddBooking(account.UserId);

            sut.Complete(calendarEvent, new List<Account>() { account }.AsReadOnly());

            Assert.Equal(CalendarEventStatus.Completed, calendarEvent.Status);
            Assert.Equal(BookingStatus.Completed, account.Bookings.First(booking => booking.UserId == account.UserId).Status);
        }
    }
}
