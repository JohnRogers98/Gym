using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.TelegramAuthContext.ValueObjects;

namespace Gym.Domain.TelegramAuthContext
{
    public class TelegramAuth : AggregateRoot
    {
        public TelegramId Id { get; }
        
        public UserId UserId { get; }

        public TelegramUsername? TelegramUsername { get; }

        private TelegramAuth(TelegramId id, UserId userId, TelegramUsername? telegramUsername)
        {
            Id = id;
            UserId = userId;
            TelegramUsername = telegramUsername;
        }

        public static TelegramAuth Create(TelegramId id, UserId userId, TelegramUsername? telegramUsername)
        {
            TelegramAuth user = new(id, userId, telegramUsername);
            return user;
        }

        public static TelegramAuth Restore(TelegramId id, UserId userId, TelegramUsername? telegramUsername)
            => new(id, userId, telegramUsername);

        public override String ToString()
            => $"{nameof(UserId)}: {UserId} \t {nameof(Id)}: {Id.Value}";

        public override Boolean Equals(Object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj is TelegramAuth other && Id == other.Id;
        }

        public override Int32 GetHashCode() => Id.GetHashCode();
    }
}
