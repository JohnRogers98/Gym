using Gym.Domain.PersonalTrainingContext.ValueObjects;

namespace Gym.Domain.PersonalTrainingContext
{
    public interface IPersonalTrainingRepository
    {
        PersonalTrainingId NextIdentity();
        Task SaveAsync(PersonalTraining personalTraining, CancellationToken cancellationToken);
        Task<PersonalTraining?> GetByIdAsync(PersonalTrainingId id, CancellationToken cancellationToken);
        Task<Boolean> ExistsAsync(PersonalTrainingId id, CancellationToken cancellationToken);
    }
}
