using Gym.Application.Extensions;
using Gym.Domain._Shared;
using Gym.Domain.CalendarEventContext;
using Gym.Domain.CalendarEventContext.Errors;
using Gym.Domain.CalendarEventContext.Events;
using Gym.Domain.CalendarEventContext.ValueObjects;

namespace Gym.Domain.Tests.CalendarEventContext
{
    public class CalendarEventTests
    {
        private readonly FakeDataFixture _fakeDataFixture = new ();

        [Fact]
        public void Check_New_Booking()
        {
            CalendarEvent sut = _fakeDataFixture.CreateCalendarEvent();
            UserId userId = _fakeDataFixture.GenerateUserId();

            var result = sut.AddBooking(userId);

            Assert.True(result.Success);
            Assert.Single(sut.Bookings);
            Assert.Single(sut.Bookings, (bookedUserId) => bookedUserId == userId);
            Assert.NotEqual(default, sut.DomainEvents.OfType<CalendarEventBookedDomainEvent>().SingleOrDefault());
        }

        [Fact]
        public void Booking_Error_When_Going_To_Make_Already_Existing_Booking_Again()
        {
            CalendarEvent sut = _fakeDataFixture.CreateCalendarEvent();
            UserId userId = _fakeDataFixture.GenerateUserId();

            var result = sut.AddBooking(userId)
                .Bind(() => sut.AddBooking(userId));

            Assert.False(result.Success);
            Assert.IsType<UserAlreadyBookedError>(result.Error);
        }

        [Fact]
        public void Check_If_Can_Book_When_Free_Space_Exists()
        {
            CalendarEvent sut = _fakeDataFixture.CreateCalendarEvent(capacity: 2);
            UserId userId = _fakeDataFixture.GenerateUserId();

            sut.AddBooking(userId);

            Assert.True(sut.HasFreeSpace());
        }

        [Fact]
        public void Check_If_Can_Not_Book_When_Event_Booking_Is_Full()
        {
            CalendarEvent sut = _fakeDataFixture.CreateCalendarEvent(capacity: 2);

            sut.AddBooking(_fakeDataFixture.GenerateUserId())
                .Bind(() => sut.AddBooking(_fakeDataFixture.GenerateUserId()));

            Assert.False(sut.HasFreeSpace());
        }

        [Fact]
        public void Check_If_Has_Already_Booking_For_User()
        {
            CalendarEvent sut = _fakeDataFixture.CreateCalendarEvent(capacity: 2);
            UserId userId = _fakeDataFixture.GenerateUserId();

            Assert.False(sut.HasBookingFor(userId));
            sut.AddBooking(userId);
            Assert.True(sut.HasBookingFor(userId));
        }

        [Fact]
        public void Check_Whether_Future_Event_Is_Not_Expired()
        {
            CalendarEvent sut = _fakeDataFixture.CreateCalendarEvent(isExpired: false);

            Assert.False(sut.HasExpired(DateTime.Now));
        }

        [Fact]
        public void Check_Whether_Past_Event_Is_Expired()
        {
            CalendarEvent sut = _fakeDataFixture.CreateCalendarEvent(isExpired: true);

            Assert.True(sut.HasExpired(DateTime.Now));
        }

        [Fact]
        public void Check_Whether_Status_Upcoming_When_Created()
        {
            CalendarEvent sut = _fakeDataFixture.CreateCalendarEvent(isExpired: false);

            Assert.Equal(CalendarEventStatus.Upcoming, sut.Status);
        }


        [Fact]
        public void Check_Whether_Status_Completed_When_Complete()
        {
            CalendarEvent sut = _fakeDataFixture.CreateCalendarEvent(isExpired: true);

            var result = sut.Complete();

            Assert.True(result.Success);
            Assert.Equal(CalendarEventStatus.Completed, sut.Status);
            Assert.NotEqual(default, sut.DomainEvents.OfType<CalendarEventCompletedDomainEvent>().SingleOrDefault());
        }

        [Fact]
        public void Check_Whether_Status_Cancelled_When_Cancel()
        {
            CalendarEvent sut = _fakeDataFixture.CreateCalendarEvent(isExpired: true);

            var result = sut.Cancel();

            Assert.True(result.Success);
            Assert.Equal(CalendarEventStatus.Cancelled, sut.Status);
            Assert.NotEqual(default, sut.DomainEvents.OfType<CalendarEventCancelledDomainEvent>().SingleOrDefault());
        }

        [Fact]
        public void Check_Whether_Cancelling_Returns_Error_When_Cancelled_Already()
        {
            CalendarEvent sut = _fakeDataFixture.CreateCalendarEvent(isExpired: false);

            var result = sut.Cancel()
                .Bind(sut.Complete);

            Assert.IsType<EventStatusIncorrectForOperationError>(result.Error);
        }

        [Fact]
        public void Check_Whether_Cancelling_Returns_Error_When_Completed_Already()
        {
            CalendarEvent sut = _fakeDataFixture.CreateCalendarEvent(isExpired: false);

            var result = sut.Complete()
                .Bind(sut.Complete);

            Assert.IsType<EventStatusIncorrectForOperationError>(result.Error);
        }

        [Fact]
        public void Check_Whether_Completing_Returns_Error_When_Cancelled_Already()
        {
            CalendarEvent sut = _fakeDataFixture.CreateCalendarEvent(isExpired: false);

            var result = sut.Cancel()
                .Bind(sut.Complete);

            Assert.IsType<EventStatusIncorrectForOperationError>(result.Error);
        }

        [Fact]
        public void Check_Whether_Completing_Returns_Error_When_Completing_Already()
        {
            CalendarEvent sut = _fakeDataFixture.CreateCalendarEvent(isExpired: false);

            var result = sut.Complete()
                .Bind(sut.Complete);

            Assert.IsType<EventStatusIncorrectForOperationError>(result.Error);
        }
    }
}
