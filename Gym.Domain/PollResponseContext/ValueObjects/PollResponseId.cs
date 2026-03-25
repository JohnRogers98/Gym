using Gym.Domain._Shared;
using Gym.Domain.PollContext.ValueObjects;

namespace Gym.Domain.PollResponseContext.ValueObjects
{
    public record PollResponseId
    {
        public String Value { get; }

        private PollResponseId(String value) => Value = value;

        public static PollResponseId From(UserId userId, PollId pollId)
        {
            return new($"{userId}_{pollId}");
        }

        public override String ToString() => Value.ToString();
    }
}
