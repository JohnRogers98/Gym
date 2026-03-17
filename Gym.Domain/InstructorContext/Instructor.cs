using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.InstructorContext.Events;
using Gym.Domain.InstructorContext.ValueObjects;

namespace Gym.Domain.InstructorContext
{
    public class Instructor : AggregateRoot
    {
        public InstructorId Id { get; }
        public FirstName FirstName { get; private set; }
        public LastName? LastName { get; private set; }

        private Instructor(InstructorId id, FirstName firstName, LastName? lastName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }

        public static Instructor Create(InstructorId id, FirstName firstName, LastName? lastName)
        {
            Instructor instructor = new(id, firstName, lastName);
            instructor.AddDomainEvent(InstructorCreatedDomainEvent.Create(instructor.Id));
            return instructor;
        } 

        public static Instructor Restore(InstructorId id, FirstName firstName, LastName? lastName)
            => new Instructor(id, firstName, lastName);

        public override String ToString() 
            => $"{nameof(Id)}: {Id} \t {nameof(FirstName)}: {FirstName} \t {nameof(LastName)}: {LastName?.Value ?? "_"}";

        public override Boolean Equals(Object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj is Instructor other && Id == other.Id;
        }

        public override Int32 GetHashCode() => Id.GetHashCode();
    }
}
