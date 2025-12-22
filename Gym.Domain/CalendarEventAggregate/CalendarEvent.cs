namespace Gym.Domain.CalendarEventAggregate
{
    public class CalendarEvent
    {
        public CalendarEventId Id { get; }

        public DateTime Start { get; private set; }
        public DateTime? End { get; private set; }

        public Int32? MaxClientCount { get; private set; }

        public TrainingInfo Training { get; private set; }

        public IEnumerable<InstructorInfo>? Instructors { get; private set; }

        public CalendarEvent(CalendarEventId id, DateTime start, DateTime? end, TrainingInfo training, Int32? maxClientCount = default, IEnumerable<InstructorInfo>? instructors = default)
        {
            Id = id;
            Start = start;
            End = end;
            Training = training;
            MaxClientCount = maxClientCount;
            Instructors = instructors;
        }

        public static CalendarEvent Create(CalendarEventId id, DateTime start, DateTime? end, TrainingInfo training, Int32? maxClientCount = default, IEnumerable<InstructorInfo>? instructors = default)
            => new CalendarEvent(id, start, end, training, maxClientCount, instructors);

        public static CalendarEvent Restore(CalendarEventId id, DateTime start, DateTime? end, TrainingInfo training, Int32? maxClientCount = default, IEnumerable<InstructorInfo>? instructors = default)
            => new CalendarEvent(id, start, end, training, maxClientCount, instructors);

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
