using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.CalendarEventContext.Errors;
using Gym.Domain.CalendarEventContext.Events;
using Gym.Domain.CalendarEventContext.ValueObjects;
using Gym.Domain.InstructorContext.ValueObjects;
using Gym.Domain.PollContext.ValueObjects;
using Gym.Domain.TrainingContext.ValueObjects;

namespace Gym.Domain.CalendarEventContext
{
    public class CalendarEvent : AggregateRoot
    {
        public CalendarEventId Id { get; }

        public TrainingPeriod TrainingPeriod { get; private set; }

        public CalendarEventStatus Status { get; private set; }

        public Capacity Capacity { get; private set; }

        public TrainingId TrainingId { get; private set; }

        private HashSet<UserId> _bookings = new();
        public IReadOnlyCollection<UserId> Bookings => _bookings.AsReadOnly();

        private List<InstructorId>? _instructors;
        public IReadOnlyCollection<InstructorId>? Instructors => _instructors?.AsReadOnly();

        public PollId? PollId { get; private set; }

        private CalendarEvent(
            CalendarEventId id,
            TrainingPeriod trainingPeriod,
            CalendarEventStatus status,
            Capacity capacity,
            TrainingId trainingId,
            IEnumerable<UserId> bookings,
            IEnumerable<InstructorId>? instructors = default,
            PollId? pollId = default)
        {
            Id = id;
            TrainingPeriod = trainingPeriod;
            Status = status;
            Capacity = capacity;
            TrainingId = trainingId;
            _bookings = bookings.ToHashSet();
            _instructors = instructors?.ToList();
            PollId = pollId;
        }

        public static CalendarEvent Create(
            CalendarEventId id, 
            TrainingPeriod trainingPeriod, 
            Capacity capacity, 
            TrainingId trainingId, 
            IEnumerable<UserId>? bookings = default, 
            IEnumerable<InstructorId>? instructors = default,
            PollId? pollId = default
            )
        {
            CalendarEvent calendarEvent = new (
                id,
                trainingPeriod,
                CalendarEventStatus.Upcoming,
                capacity,
                trainingId,
                bookings ?? new HashSet<UserId>(),
                instructors,
                pollId
            );
            
            calendarEvent.AddDomainEvent(CalendarEventCreatedDomainEvent.Create(calendarEvent.Id));
            return calendarEvent;
        }

        public static CalendarEvent Restore(
            CalendarEventId id,
            TrainingPeriod trainingPeriod,
            CalendarEventStatus status,
            Capacity capacity,
            TrainingId trainingId,
            IEnumerable<UserId> bookings,
            IEnumerable<InstructorId>? instructors = default,
            PollId? pollId = default)
        {
            return new (id, trainingPeriod, status, capacity, trainingId, bookings, instructors, pollId);
        }

        public Result AddBooking(UserId userId)
        {
            if (HasFreeSpace() is false)
                return Result.Fail(EventHasNotFreeSpaceError.Create(Id));

            var wasAdded = _bookings.Add(userId);
            if (wasAdded is not true)
                return Result.Fail(UserAlreadyBookedError.Create(Id, userId));

            base.AddDomainEvent(CalendarEventBookedDomainEvent.Create(Id, userId));

            return Result.Ok();
        }
        
        public Boolean HasFreeSpace() => Capacity.Value > _bookings.Count;

        public Boolean HasExpired(DateTime checkPoint) => checkPoint > TrainingPeriod.StartsAt.Value;

        public Int32 BookingCount() => _bookings.Count;

        public Boolean HasBookingFor(UserId userId) => _bookings.Any(anUserId => anUserId == userId);

        internal Result Complete()
        {
            if(Status is not CalendarEventStatus.Upcoming)
            {
                return Result.Fail(EventStatusIncorrectForOperationError.Create(Id));
            }

            Status = CalendarEventStatus.Completed;
            base.AddDomainEvent(CalendarEventCompletedDomainEvent.Create(Id, Bookings));

            return Result.Ok();
        }

        internal Result Cancel()
        {
            if (Status is not CalendarEventStatus.Upcoming)
            {
                return Result.Fail(EventStatusIncorrectForOperationError.Create(Id));
            }

            Status = CalendarEventStatus.Cancelled;
            base.AddDomainEvent(CalendarEventCancelledDomainEvent.Create(Id, Bookings));

            return Result.Ok();
        }

        public override String ToString()
            => $"{nameof(Id)}: {Id} \t {nameof(TrainingPeriod)}: {TrainingPeriod} \t {nameof(TrainingId)}: {TrainingId}";

        public override Boolean Equals(Object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj is CalendarEvent other && Id == other.Id;
        }

        public override Int32 GetHashCode() => Id.GetHashCode();
    }
}
