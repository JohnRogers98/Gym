using Gym.Domain._Common;
using Gym.Domain.TrainingContext.Events;

namespace Gym.Domain.TrainingContext
{
    public class Training : AggregateRoot
    {
        public TrainingId Id { get; }
        public String Name { get; private set; }
        public String? Description { get; private set; }

        public Training(TrainingId id, String name, String? description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public static Training Create(TrainingId id, String name, String? description)
        {
            Training training = new (id, name, description);
            training.AddDomainEvent(TrainingCreatedDomainEvent.Create(training.Id));
            return training;
        }

        public static Training Restore(TrainingId id, String name, String? description)
            => new Training(id, name, description);

        public override String ToString() => $"{nameof(Id)}: {Id} \t {nameof(Name)}: {Name} \t {nameof(Description)}: {Description ?? "_"}";

        public override Boolean Equals(Object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj is Training other && Id == other.Id;
        }

        public override Int32 GetHashCode() => Id.GetHashCode();
    }
}
