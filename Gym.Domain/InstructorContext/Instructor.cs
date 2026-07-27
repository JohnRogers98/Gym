using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.InstructorContext.Events;
using Gym.Domain.InstructorContext.ValueObjects;

namespace Gym.Domain.InstructorContext
{
    public class Instructor : AggregateRoot
    {
        public InstructorId Id { get; }
        public UserId UserId { get; }

        private Instructor(InstructorId id, UserId userId)
        {
            Id = id;
            UserId = userId;
        }

        public static Instructor Create(InstructorId id, UserId userId)
        {
            Instructor instructor = new(id, userId);
            instructor.AddDomainEvent(InstructorCreatedDomainEvent.Create(instructor.Id));
            return instructor;
        } 

        public static Instructor Restore(InstructorId id, UserId userId)
            => new Instructor(id, userId);

        public override String ToString() 
            => $"{nameof(Id)}: {Id} \t {nameof(UserId)}: {UserId}";

        public override Boolean Equals(Object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj is Instructor other && Id == other.Id;
        }

        public override Int32 GetHashCode() => Id.GetHashCode();
    }
}
