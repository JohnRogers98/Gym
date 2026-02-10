using Gym.Domain.TrainingContext;
using Gym.Infrastructure.Entities.Repositories.Trainings;

namespace Gym.Infrastructure.Entities.Extensions.Mappings
{
    internal static class TrainingExtensions
    {
        public static Training ToDomain(this TrainingEntity entity)
        {
            return Training.Restore(TrainingId.From(entity.Id.ToString()), entity.Name, entity.Description);
        }

        public static TrainingEntity ToEntity(this Training training)
        {
            return new() { Id = training.Id.Value.ToObjectId(), Name = training.Name, Description = training.Description };
        }
    }
}
