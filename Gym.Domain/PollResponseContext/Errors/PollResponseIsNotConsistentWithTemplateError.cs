using Gym.Domain._Common;
using Gym.Domain.PollContext.ValueObjects;

namespace Gym.Domain.PollResponseContext.Errors
{
    public class PollResponseIsNotConsistentWithTemplateError : DomainError
    {
        public PollId PollId { get; }
        public PollResponse PollResponse { get; }

        private PollResponseIsNotConsistentWithTemplateError(PollId pollId, PollResponse pollResponse) : base(nameof(PollResponseIsNotConsistentWithTemplateError))
        {
            PollId = pollId;
            PollResponse = pollResponse;
        }

        public static PollResponseIsNotConsistentWithTemplateError Create(PollId pollId, PollResponse pollResponse) 
            => new(pollId, pollResponse);

        public override String GetErrorMessage() => $"Poll id - {PollId.Value} inconsistent with response.";
    }
}
