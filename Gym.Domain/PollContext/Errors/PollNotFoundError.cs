using Gym.Domain._Common;
using Gym.Domain.PollContext.ValueObjects;

namespace Gym.Domain.InstructorContext.Errors
{
    public class PollNotFoundError : DomainError
    {
        public PollId PollId { get; }

        private PollNotFoundError(PollId pollId) : base(nameof(PollNotFoundError)) 
        {
            PollId = pollId;
        }

        public static PollNotFoundError Create(PollId pollId) => new(pollId);

        public override String GetErrorMessage() => $"Poll with id - {PollId.Value} not found.";
    }
}
