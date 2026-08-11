namespace Gym.Abstractions.Query.PersonalTrainings
{
    public interface IPersonalTrainingProjectionQueryService
    {
        Task<PersonalTrainingProjection?> GetByIdAsync(String personalTrainingId, CancellationToken cancellationToken);
        Task<IEnumerable<PersonalTrainingProjection>> GetAllByInstructorIdAsync(String instructorId, CancellationToken cancellationToken);
        Task<IEnumerable<PersonalTrainingProjection>> GetAllByClientIdAsync(String clientId, CancellationToken cancellationToken);

        Task<IEnumerable<PersonalTrainingProjection>> GetAllAsync(CancellationToken cancellationToken);
    }
}
