using Gym.Domain._Common;
using Gym.Domain._Exceptions;
using Gym.Domain._Shared;
using Gym.Domain.CalendarEventAggregate.Errors;

namespace Gym.Domain.CalendarEventAggregate
{
    public class CalendarEvent : AggregateRoot
    {
        public CalendarEventId Id { get; }

        public DateTime Start { get; private set; }
        public DateTime? End { get; private set; }

        public Int32? MaxClientCount { get; private set; }

        public TrainingInfo Training { get; private set; }

        private List<InstructorInfo>? _instructors;
        public IReadOnlyCollection<InstructorInfo>? Instructors => _instructors?.AsReadOnly();

        private HashSet<UserId> _bookings = new();
        public IReadOnlyCollection<UserId> Bookings => _bookings.AsReadOnly();

        private CalendarEvent(CalendarEventId id, DateTime start, DateTime? end, TrainingInfo training,
            IEnumerable<UserId> bookings, Int32? maxClientCount = default, IEnumerable<InstructorInfo>? instructors = default)
        {
            Id = id;
            Start = start;
            End = end;
            Training = training;
            MaxClientCount = maxClientCount;
            _instructors = instructors?.ToList();
            _bookings = bookings.ToHashSet();
        }

        public static CalendarEvent Create(
            CalendarEventId id, DateTime start, DateTime? end, TrainingInfo training, IEnumerable<UserId>? bookings = default, Int32? maxClientCount = default,
            IEnumerable<InstructorInfo>? instructors = default)
        {
            return new (id, start, end, training, bookings ?? new HashSet<UserId>(), maxClientCount, instructors);
        }

        public static CalendarEvent Restore(CalendarEventId id, DateTime start, DateTime? end, TrainingInfo training,
            IEnumerable<UserId> bookings, Int32? maxClientCount = default, IEnumerable<InstructorInfo>? instructors = default)
        {
            return new (id, start, end, training, bookings, maxClientCount, instructors);
        }

        public void AddBooking(UserId userId)
        {
            if (HasFreeSpace() is false) 
                throw new DomainException(EventHasNotFreeSpaceError.Create(Id));

            var wasAdded = _bookings.Add(userId);
            if (wasAdded is not true) 
                throw new DomainException(UserAlreadyBookedError.Create(Id, userId));
        }
        
        public Boolean HasFreeSpace() => MaxClientCount is null || MaxClientCount > _bookings.Count;

        public Boolean HasExpired(DateTime checkPoint) => checkPoint > Start;

        public Int32 BookingCount() => _bookings.Count;

        public Boolean HasBookingFor(UserId userId) => _bookings.Where(anUserId => anUserId == userId).Any();

        public override String ToString()
            => $"{nameof(Id)}: {Id} \t {nameof(Start)}: {Start} \t {nameof(End)}: {End?.ToString() ?? "_"} \t {nameof(Training)}: {Training.Name}";

        public override Boolean Equals(Object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj is CalendarEvent other && Id == other.Id;
        }

        public override Int32 GetHashCode() => Id.GetHashCode();
    }
}
