namespace Gym.Abstractions.Query.Instructors
{
    public interface IInstructorProjectionQueryService
    {
        Task<InstructorProjection?> GetByIdAsync(String instructorId, CancellationToken cancellationToken);

        Task<IEnumerable<InstructorProjection>> GetAllAsync(CancellationToken cancellationToken);
    }
}
