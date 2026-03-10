using Gym.Domain.AccountContext.Events;

namespace Gym.Domain.AccountContext
{
    public partial class Account
    {
        public void ApplyEvent(TrainingBookedDomainEvent @event)
        {
            Booking booking = Booking.Restore(@event.BookingId, @event.UserId, @event.CalendarEventId, BookingStatus.Upcoming);
            _bookings.Add(booking);
            AvailableTrainingsCount--;
        }

        public void ApplyEvent(TrainingCancelledDomainEvent @event)
        {
            Booking booking = this.FindBookingByCalendarEvent(@event.CalendarEventId)!;
            booking.Cancel();
            AvailableTrainingsCount++;
        }

        public void ApplyEvent(TrainingRebookedDomainEvent @event)
        {
            Booking booking = this.FindBookingByCalendarEvent(@event.CalendarEventId)!;
            booking.Rebook();
            AvailableTrainingsCount--;
        }

        public void ApplyEvent(TrainingCompletedDomainEvent @event)
        {
            Booking booking = this.FindBookingByCalendarEvent(@event.CalendarEventId)!;
            booking.MarkAsCompleted();
        }

        public void ApplyEvent(AccountChargedDomainEvent @event)
        {
            AvailableTrainingsCount += @event.ByCount;
        }

        public void ApplyEvent(AccountCreatedDomainEvent @event) { }
    }
}
