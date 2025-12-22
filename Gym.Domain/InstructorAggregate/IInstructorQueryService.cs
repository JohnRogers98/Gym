namespace Gym.Domain.InstructorAggregate
{
    public interface IInstructorQueryService
    {
        Task<Instructor?> GetByIdAsync(InstructorId id, CancellationToken cancellationToken);
        Task<IEnumerable<Instructor>> GetAllAsync(CancellationToken cancellationToken);
    }
}
