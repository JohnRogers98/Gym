using Gym.Domain._Exceptions;
using Gym.Domain._Shared;
using Gym.Domain.CalendarEventContext;

namespace Gym.Domain.Tests
{
    public class CalendarEventTests
    {
        private readonly FakeDataFixture _fakeDataFixture = new ();

        [Fact]
        public void Check_New_Booking()
        {
            CalendarEvent calendarEvent = _fakeDataFixture.CreateCalendarEvent();
            UserId userId = UserId.From(Guid.NewGuid().ToString());

            calendarEvent.AddBooking(userId);

            Assert.Single(calendarEvent.Bookings);
            Assert.Single(calendarEvent.Bookings, (bookedUserId) => bookedUserId == userId);
        }

        [Fact]
        public void Booking_Throws_When_Going_To_Make_Already_Existing_Booking_Again()
        {
            CalendarEvent calendarEvent = _fakeDataFixture.CreateCalendarEvent();
            UserId userId = UserId.From(Guid.NewGuid().ToString());

            calendarEvent.AddBooking(userId);
            Assert.Throws<DomainException>(() => calendarEvent.AddBooking(userId));
        }

        [Fact]
        public void Check_If_Can_Book_When_Free_Space_Exists()
        {
            CalendarEvent calendarEvent = _fakeDataFixture.CreateCalendarEvent(maxClientCount: 2);

            calendarEvent.AddBooking(UserId.From(Guid.NewGuid().ToString()));

            Assert.True(calendarEvent.HasFreeSpace());
        }

        [Fact]
        public void Check_If_Can_Not_Book_When_Event_Booking_Is_Full()
        {
            CalendarEvent calendarEvent = _fakeDataFixture.CreateCalendarEvent(maxClientCount: 2);

            calendarEvent.AddBooking(UserId.From(Guid.NewGuid().ToString()));
            calendarEvent.AddBooking(UserId.From(Guid.NewGuid().ToString()));

            Assert.False(calendarEvent.HasFreeSpace());
        }

        [Fact]
        public void Check_If_Has_Already_Booking_For_User()
        {
            CalendarEvent calendarEvent = _fakeDataFixture.CreateCalendarEvent(maxClientCount: 2);
            UserId userId = UserId.From(Guid.NewGuid().ToString());

            Assert.False(calendarEvent.HasBookingFor(userId));
            calendarEvent.AddBooking(userId);
            Assert.True(calendarEvent.HasBookingFor(userId));
        }

        [Fact]
        public void Check_Whether_Future_Event_Is_Not_Expired()
        {
            CalendarEvent calendarEvent = _fakeDataFixture.CreateCalendarEvent(isExpired: false);

            Assert.False(calendarEvent.HasExpired(DateTime.Now));
        }

        [Fact]
        public void Check_Whether_Past_Event_Is_Expired()
        {
            CalendarEvent calendarEvent = _fakeDataFixture.CreateCalendarEvent(isExpired: true);

            Assert.True(calendarEvent.HasExpired(DateTime.Now));
        }
    }
}
