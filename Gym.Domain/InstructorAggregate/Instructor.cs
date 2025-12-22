namespace Gym.Domain.InstructorAggregate
{
    public class Instructor
    {
        public InstructorId Id { get; }
        public String FirstName { get; private set; }
        public String? LastName { get; private set; }

        private Instructor(InstructorId id, String firstName, String? lastName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }

        public static Instructor Create(InstructorId id, String firstName, String? lastName) 
            => new Instructor(id, firstName, lastName);

        public static Instructor Restore(InstructorId id, String firstName, String? lastName)
            => new Instructor(id, firstName, lastName);

        public override String ToString() 
            => $"{nameof(Id)}: {Id} \t {nameof(FirstName)}: {FirstName} \t {nameof(LastName)}: {LastName ?? "_"}";

        public override Boolean Equals(Object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj is Instructor other && Id == other.Id;
        }

        public override Int32 GetHashCode() => Id.GetHashCode();
    }
}
