using Gym.Domain._Common;
using Gym.Domain.InstructorContext.ValueObjects;

namespace Gym.Domain.InstructorContext.Errors
{
    public class InstructorNotFoundError : DomainError
    {
        public InstructorId InstructorId { get; }

        private InstructorNotFoundError(InstructorId instructorId) : base(nameof(InstructorNotFoundError)) 
        {
            InstructorId = instructorId;
        }

        public static InstructorNotFoundError Create(InstructorId instructorId) => new(instructorId);

        public override String GetErrorMessage() => $"Instructor with id - {InstructorId.Value} not found.";
    }
}
