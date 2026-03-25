using Gym.Domain._Common;

namespace Gym.Domain.PollResponseContext.Errors
{
    public class PollResponseRequiredError : DomainError
    {
        private PollResponseRequiredError() : base(nameof(PollResponseRequiredError)) { }

        public static PollResponseRequiredError Create() => new();

        public override String GetErrorMessage() => $"Poll response required.";
    }
}
