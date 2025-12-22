namespace Gym.Domain.TrainingAggregate
{
    public interface ITrainingQueryService
    {
        Task<Training?> GetByIdAsync(TrainingId id, CancellationToken cancellationToken);
        Task<IEnumerable<Training>> GetAllAsync(CancellationToken cancellationToken);
    }
}
