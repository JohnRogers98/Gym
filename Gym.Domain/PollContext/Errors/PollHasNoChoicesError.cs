using Gym.Domain._Common;

namespace Gym.Domain.PollContext.Errors
{
    public class PollHasNoChoicesError : DomainError
    {
        private PollHasNoChoicesError() : base(nameof(PollHasNoChoicesError)) { }

        public static PollHasNoChoicesError Create() => new();

        public override String GetErrorMessage() => $"Poll has no choices.";
    }
}
