using Gym.Domain.TrainingAggregate;

namespace Gym.Domain.CalendarEventAggregate
{
    public record TrainingInfo
    {
        public TrainingId Id { get; private set; }
        public String Name { get; private set; }
        public String? Description { get; private set; }

        private TrainingInfo(TrainingId id, String name, String? description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public static TrainingInfo From(Training training) 
            => new TrainingInfo(training.Id, training.Name, training.Description);

        public static TrainingInfo Create(TrainingId id, String name, String? description)
            => new TrainingInfo(id, name, description);
    }
}
