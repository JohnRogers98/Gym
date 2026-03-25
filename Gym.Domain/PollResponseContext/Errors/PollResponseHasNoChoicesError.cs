using Gym.Domain._Common;

namespace Gym.Domain.PollResponseContext.Errors
{
    public class PollResponseHasNoChoicesError : DomainError
    {
        public PollResponse PollResponse { get; }

        private PollResponseHasNoChoicesError(PollResponse pollResponse) : base(nameof(PollResponseHasNoChoicesError))
        {
            PollResponse = pollResponse;
        }

        public static PollResponseHasNoChoicesError Create(PollResponse pollResponse) => new(pollResponse);

        public override String GetErrorMessage() => $"Poll response has no choices.";
    }
}
