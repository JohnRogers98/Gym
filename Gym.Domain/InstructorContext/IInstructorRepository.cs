namespace Gym.Domain.InstructorContext
{
    public interface IInstructorRepository
    {
        InstructorId NextIdentity();
        Task SaveAsync(Instructor instructor, CancellationToken cancellationToken);
        Task<Instructor?> GetByIdAsync(InstructorId id, CancellationToken cancellationToken);
        Task<Boolean> ExistsAsync(InstructorId id, CancellationToken cancellationToken);
    }
}
