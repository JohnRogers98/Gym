using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.TrainingContext.Events;
using Gym.Domain.TrainingContext.ValueObjects;

namespace Gym.Domain.TrainingContext
{
    public class Training : AggregateRoot
    {
        public TrainingId Id { get; }
        public TrainingName Name { get; private set; }
        public Description? Description { get; private set; }

        public Training(TrainingId id, TrainingName trainingName, Description? description)
        {
            Id = id;
            Name = trainingName;
            Description = description;
        }

        public static Training Create(TrainingId id, TrainingName trainingName, Description? description)
        {
            Training training = new (id, trainingName, description);
            training.AddDomainEvent(TrainingCreatedDomainEvent.Create(training.Id));
            return training;
        }

        public static Training Restore(TrainingId id, TrainingName trainingName, Description? description)
            => new Training(id, trainingName, description);

        public override String ToString() => $"{nameof(Id)}: {Id} \t {nameof(Name)}: {Name} \t {nameof(Description)}: {Description?.Value ?? "_"}";

        public override Boolean Equals(Object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj is Training other && Id == other.Id;
        }

        public override Int32 GetHashCode() => Id.GetHashCode();
    }
}
