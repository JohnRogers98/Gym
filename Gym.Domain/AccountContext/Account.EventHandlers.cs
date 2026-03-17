using Gym.Domain.AccountContext.Entities;
using Gym.Domain.AccountContext.Events;
using Gym.Domain.AccountContext.ValueObjects;

namespace Gym.Domain.AccountContext
{
    public partial class Account
    {
        public void ApplyEvent(TrainingBookedDomainEvent @event)
        {
            Booking booking = Booking.Restore(@event.BookingId, @event.UserId, @event.CalendarEventId, BookingStatus.Upcoming);
            _bookings.Add(booking);
            this.DecrementRemainingTrainings();
        }

        public void ApplyEvent(TrainingCancelledDomainEvent @event)
        {
            Booking booking = this.FindBookingByCalendarEvent(@event.CalendarEventId)!;
            booking.Cancel();
            this.IncrementRemainingTrainings();
        }

        public void ApplyEvent(TrainingRebookedDomainEvent @event)
        {
            Booking booking = this.FindBookingByCalendarEvent(@event.CalendarEventId)!;
            booking.Rebook();
            this.DecrementRemainingTrainings();
        }

        public void ApplyEvent(TrainingCompletedDomainEvent @event)
        {
            Booking booking = this.FindBookingByCalendarEvent(@event.CalendarEventId)!;
            booking.MarkAsCompleted();
        }

        public void ApplyEvent(AccountChargedDomainEvent @event)
        {
            this.IncrementRemainingTrainings(@event.ByCount);
        }

        public void ApplyEvent(AccountCreatedDomainEvent @event) { }
    }
}
