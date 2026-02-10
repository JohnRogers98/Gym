using Gym.Domain._Common;
using Gym.Domain._Shared;

namespace Gym.Domain.UserContext
{
    public class User : AggregateRoot
    {
        public UserId Id { get; }
        public TelegramId? TelegramId { get; private set; }
        public UserRole Role { get; private set; }

        private User(UserId id, UserRole role, TelegramId? telegramId)
        {
            Id = id;
            TelegramId = telegramId;
            Role = role;
        }

        public static User Create(UserId id, UserRole role, TelegramId? telegramId)
        {
            User user = new(id, role, telegramId);
            return user;
        }

        public static User Restore(UserId id, UserRole role, TelegramId? telegramId)
            => new(id, role, telegramId);

        public override String ToString()
            => $"{nameof(Id)}: {Id} \t {nameof(Role)}: {Role} \t {nameof(TelegramId)}: {TelegramId?.Value.ToString() ?? "_"}";

        public override Boolean Equals(Object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj is User other && Id == other.Id;
        }

        public override Int32 GetHashCode() => Id.GetHashCode();
    }
}
