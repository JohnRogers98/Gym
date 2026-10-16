using Gym.Chat.Domain.SharedContext;
using Gym.Domain.ChatContext.ValueObjects;

namespace Gym.Domain.ChatContext.Entities
{
    public class Participant
    {
        public UserId UserId { get; }

        public MessageSequence LastReadSequence { get; private set; }

        public Participant(UserId userId, MessageSequence lastReadSequence)
        {
            UserId = userId;
            LastReadSequence = lastReadSequence;
        }

        public static Participant Create(UserId userId)
            => new(userId, MessageSequence.Zero);

        public static Participant Restore(UserId userId, MessageSequence lastReadSequence)
            => new(userId, lastReadSequence);

        public override String ToString()
            => $"{nameof(UserId)}: {UserId} \t {nameof(LastReadSequence)}: {LastReadSequence}";

        public override Boolean Equals(Object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj is Participant other && UserId == other.UserId;
        }

        public override Int32 GetHashCode() => UserId.GetHashCode();
    }
}
