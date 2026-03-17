using Gym.Domain._Common;

namespace Gym.Domain.InstructorContext.Errors
{
    public class InstructorIdValidationError : DomainError
    {
        private InstructorIdValidationError() : base(nameof(InstructorIdValidationError)) { }

        public static InstructorIdValidationError Create() => new();

        public override String GetErrorMessage() => $"Instructor id is invalid.";
    }
}
