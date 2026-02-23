namespace Gym.Abstractions.Query.Trainings
{
    public interface ITrainingProjectionQueryService
    {
        Task<TrainingProjection?> GetByIdAsync(String trainingId, CancellationToken cancellationToken);

        Task<IEnumerable<TrainingProjection>> GetAllAsync(CancellationToken cancellationToken);
    }
}
