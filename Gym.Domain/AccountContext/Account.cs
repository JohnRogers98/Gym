using Gym.Domain._Common;
using Gym.Domain._Exceptions;
using Gym.Domain._Shared;
using Gym.Domain.AccountContext.Errors;
using Gym.Domain.AccountContext.Events;
namespace Gym.Domain.AccountContext
{
    public partial class Account : EventSourcedAggregateRoot
    {
        public AccountId Id { get; }

        public UserId UserId { get; }

        public Int32 AvailableTrainingsCount { get; private set; }

        private HashSet<Booking> _bookings;
        public IReadOnlyCollection<Booking> Bookings => _bookings.AsReadOnly();

        private Account(AccountId id, UserId userId)
        {
            Id = id;
            UserId = userId;
            AvailableTrainingsCount = 0;
            _bookings = new HashSet<Booking>();
        }

        public static Account Create(AccountId id, UserId userId) 
        {
            Account account = new(id, userId);
            account.AddDomainEvent(AccountCreatedDomainEvent.Create());
            return account;
        }

        public static Account Restore(AccountId id, UserId userId, IEnumerable<DomainEvent> events)
        {
            Account account = new(id, userId);
            foreach (DomainEvent @event in events)
            {
                account.ApplyEvent(@event);
            }
            return account;
        }

        internal void Charge(Int32 byCount)
        {
            if(byCount <= 0)
            {
                throw new DomainException(AccountNotChargedError.Create(UserId));
            }

            AvailableTrainingsCount += byCount;

            base.AddDomainEvent(AccountChargedDomainEvent.Create(UserId, byCount));
        }

        internal Booking MakeBooking(CalendarEventId calendarEventId)
        {
            if (this.HasCalendarEventBooking(calendarEventId))
            {
                throw new DomainException(CalendarEventAlreadyBookedError.Create(UserId, calendarEventId));
            }
            if (this.HasAvailableTraining() is false)
            {
                throw new DomainException(AccountNotChargedError.Create(UserId));
            }

            BookingId bookingId = BookingId.From(UserId, calendarEventId);
            Booking booking = Booking.Create(bookingId, UserId, calendarEventId);

            _bookings.Add(booking);
            AvailableTrainingsCount--;

            base.AddDomainEvent(TrainingBookedDomainEvent.Create(booking.Id, booking.UserId, booking.CalendarEventId));
            
            return booking;
        }

        internal void CancelBooking(CalendarEventId calendarEventId)
        {
            Booking? booking = this.FindBookingByCalendarEvent(calendarEventId);
            if (booking is null)
            {
                throw new DomainException(CalendarEventBookingNotExistError.Create(UserId, calendarEventId));
            }

            booking.Cancel();
            AvailableTrainingsCount++;

            base.AddDomainEvent(TrainingBookingCancelledDomainEvent.Create(booking.Id, UserId, calendarEventId));
        }

        internal void Rebook(CalendarEventId calendarEventId)
        {
            Booking? booking = this.FindBookingByCalendarEvent(calendarEventId);
            if (booking is null)
            {
                throw new DomainException(CalendarEventBookingNotExistError.Create(UserId, calendarEventId));
            }

            booking.Rebook();
            AvailableTrainingsCount--;

            base.AddDomainEvent(TrainingRebookedDomainEvent.Create(booking.Id, UserId, calendarEventId));
        }

        internal void CompleteBooking(CalendarEventId calendarEventId)
        {
            Booking? booking = this.FindBookingByCalendarEvent(calendarEventId);
            if (booking is null)
            {
                throw new DomainException(CalendarEventBookingNotExistError.Create(UserId, calendarEventId));
            }

            booking.MarkAsCompleted();

            base.AddDomainEvent(TrainingCompletedDomainEvent.Create(booking.Id, UserId, calendarEventId));
        }

        internal Boolean HasCalendarEventBooking(CalendarEventId calendarEventId) => 
            _bookings.Any(aBooking => aBooking.CalendarEventId == calendarEventId);

        internal Booking? FindBookingByCalendarEvent(CalendarEventId calendarEventId) =>
            _bookings.FirstOrDefault(aBooking => aBooking.CalendarEventId == calendarEventId);

        internal Boolean HasAvailableTraining() => AvailableTrainingsCount > 0;

    }
}
