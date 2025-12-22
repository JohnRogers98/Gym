namespace Gym.Domain.TrainingAggregate
{
    public interface ITrainingRepository
    {
        TrainingId NextIdentity();
        Task SaveAsync(Training training, CancellationToken cancellationToken);
        Task<Training?> GetByIdAsync(TrainingId id, CancellationToken cancellationToken);
        Task<Boolean> ExistsAsync(TrainingId id, CancellationToken cancellationToken);
    }
}
