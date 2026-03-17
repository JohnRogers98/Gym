using Gym.Application.Extensions;
using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.AccountContext.Entities;
using Gym.Domain.AccountContext.Errors;
using Gym.Domain.AccountContext.Events;
using Gym.Domain.AccountContext.ValueObjects;
using Gym.Domain.CalendarEventContext.ValueObjects;
namespace Gym.Domain.AccountContext
{
    public partial class Account : EventSourcedAggregateRoot
    {
        public AccountId Id { get; }

        public UserId UserId { get; }

        public RemainingTrainings RemainingTrainings { get; private set; }

        private HashSet<Booking> _bookings;
        public IReadOnlyCollection<Booking> Bookings => _bookings.AsReadOnly();

        private Account(AccountId id, UserId userId, RemainingTrainings remainingTrainings)
        {
            Id = id;
            UserId = userId;
            RemainingTrainings = remainingTrainings;
            _bookings = new HashSet<Booking>();
        }

        public static Account Create(AccountId id, UserId userId) 
        {
            Account account = new(id, userId, RemainingTrainings.From(0).Unwrap());
            account.AddDomainEvent(AccountCreatedDomainEvent.Create());
            return account;
        }

        public static Account Restore(AccountId id, UserId userId, IEnumerable<DomainEvent> events)
        {
            Account account = new(id, userId, RemainingTrainings.From(0).Unwrap());
            foreach (DomainEvent @event in events)
            {
                account.ApplyEvent(@event);
            }
            return account;
        }

        internal Result Charge(Int32 byCount)
        {
            if(byCount <= 0)
                return Result.Fail(AccountNotChargedError.Create(UserId));

            this.IncrementRemainingTrainings(byCount);

            base.AddDomainEvent(AccountChargedDomainEvent.Create(UserId, byCount));

            return Result.Ok();
        }

        internal Result<Booking> MakeBooking(CalendarEventId calendarEventId)
        {
            if (this.HasCalendarEventBooking(calendarEventId))
            {
                return Result<Booking>.Fail(CalendarEventAlreadyBookedError.Create(UserId, calendarEventId));
            }
            if (this.HasAvailableTraining() is false)
            {
                return Result<Booking>.Fail(AccountNotChargedError.Create(UserId));
            }

            BookingId bookingId = BookingId.From(UserId, calendarEventId);
            Booking booking = Booking.Create(bookingId, UserId, calendarEventId);

            _bookings.Add(booking);
            this.DecrementRemainingTrainings();

            base.AddDomainEvent(TrainingBookedDomainEvent.Create(booking.Id, booking.UserId, booking.CalendarEventId));
            
            return Result<Booking>.Ok(booking);
        }

        internal Result CancelBooking(CalendarEventId calendarEventId)
        {
            Booking? booking = this.FindBookingByCalendarEvent(calendarEventId);
            if (booking is null)
            {
                return Result.Fail(CalendarEventBookingNotExistError.Create(UserId, calendarEventId));
            }

            Result cancellingResult = booking.Cancel();
            if(cancellingResult.Success is false)
            {
                return cancellingResult;
            }

            this.IncrementRemainingTrainings();

            base.AddDomainEvent(TrainingCancelledDomainEvent.Create(booking.Id, UserId, calendarEventId));

            return Result.Ok();
        }

        internal Result Rebook(CalendarEventId calendarEventId)
        {
            Booking? booking = this.FindBookingByCalendarEvent(calendarEventId);
            if (booking is null)
            {
                return Result.Fail(CalendarEventBookingNotExistError.Create(UserId, calendarEventId));
            }

            Result rebookingResult = booking.Rebook();
            if (rebookingResult.Success is false)
            {
                return rebookingResult;
            }

            this.DecrementRemainingTrainings();

            base.AddDomainEvent(TrainingRebookedDomainEvent.Create(booking.Id, UserId, calendarEventId));

            return Result.Ok();
        }

        internal Result CompleteBooking(CalendarEventId calendarEventId)
        {
            Booking? booking = this.FindBookingByCalendarEvent(calendarEventId);
            if (booking is null)
            {
                return Result.Fail(CalendarEventBookingNotExistError.Create(UserId, calendarEventId));
            }

            Result markingResult = booking.MarkAsCompleted();
            if (markingResult.Success is false)
            {
                return markingResult;
            }

            base.AddDomainEvent(TrainingCompletedDomainEvent.Create(booking.Id, UserId, calendarEventId));

            return Result.Ok();
        }

        internal Boolean HasCalendarEventBooking(CalendarEventId calendarEventId) => 
            _bookings.Any(aBooking => aBooking.CalendarEventId == calendarEventId);

        internal Booking? FindBookingByCalendarEvent(CalendarEventId calendarEventId) =>
            _bookings.FirstOrDefault(aBooking => aBooking.CalendarEventId == calendarEventId);

        internal Boolean HasAvailableTraining() => RemainingTrainings.CanBook();

        private void DecrementRemainingTrainings(Int32 byCount = 1)
        {
            RemainingTrainings = RemainingTrainings.Decrement(byCount).Unwrap();
        }

        private void IncrementRemainingTrainings(Int32 byCount = 1)
        {
            RemainingTrainings = RemainingTrainings.Increment(byCount).Unwrap();
        }
    }
}
