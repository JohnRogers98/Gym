using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.UserContext.Events;

namespace Gym.Domain.UserContext
{
    public class User : AggregateRoot
    {
        public UserId Id { get; }
        public TelegramId? TelegramId { get; private set; }
        public TelegramUsername? TelegramUsername { get; private set; }
        public FirstName? FirstName { get; private set; }
        public LastName? LastName { get; private set; }
        public UserRole Role { get; private set; }

        private User(UserId id, UserRole role, TelegramId? telegramId, TelegramUsername? telegramUsername, FirstName? firstName, LastName? lastName)
        {
            Id = id;
            TelegramId = telegramId;
            Role = role;
            TelegramUsername = telegramUsername;
            FirstName = firstName;
            LastName = lastName;
        }

        public static User Create(UserId id, UserRole role, TelegramId? telegramId, TelegramUsername? telegramUsername, FirstName? firstName, LastName? lastName)
        {
            User user = new(id, role, telegramId, telegramUsername, firstName, lastName);
            user.AddDomainEvent(UserCreatedDomainEvent.Create(id));
            return user;
        }

        public static User Restore(UserId id, UserRole role, TelegramId? telegramId, TelegramUsername? telegramUsername, FirstName? firstName, LastName? lastName)
            => new(id, role, telegramId, telegramUsername, firstName, lastName);

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
