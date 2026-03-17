using Gym.Application.Extensions;
using Gym.Domain._Shared;
using Gym.Domain.TrainingContext;
using Gym.Domain.TrainingContext.ValueObjects;
using Gym.Infrastructure.Entities.Repositories.Trainings;

namespace Gym.Infrastructure.Entities.Extensions.Mappings
{
    internal static class TrainingExtensions
    {
        public static Training ToDomain(this TrainingEntity entity)
        {
            return Training.Restore(
                TrainingId.From(entity.Id.ToString()).Unwrap(),
                TrainingName.From(entity.Name).Unwrap(),
                entity.Description is not null ? Description.From(entity.Description).Unwrap() : null
            );
        }

        public static TrainingEntity ToEntity(this Training training)
        {
            return new() 
            { 
                Id = training.Id.Value.ToObjectId(),
                Name = training.Name.Value,
                Description = training.Description?.Value 
            };
        }
    }
}
