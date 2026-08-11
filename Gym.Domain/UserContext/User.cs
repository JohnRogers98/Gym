using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.UserContext.Events;
using Gym.Domain.UserContext.ValueObjects;

namespace Gym.Domain.UserContext
{
    public class User : AggregateRoot
    {
        public UserId Id { get; }
        public UserRole Role { get; }
        public FirstName? FirstName { get; }
        public LastName? LastName { get; }
        public TelegramId? TelegramId { get; }

        private User(UserId id, UserRole role, FirstName? firstName, LastName? lastName, TelegramId? telegramId)
        {
            Id = id;
            Role = role;
            FirstName = firstName;
            LastName = lastName;
            TelegramId = telegramId;
        }

        public static User Create(UserId id, UserRole role, FirstName? firstName, LastName? lastName, TelegramId? telegramId)
        {
            User user = new(id, role, firstName, lastName, telegramId);
            user.AddDomainEvent(UserCreatedDomainEvent.Create(id));
            return user;
        }

        public static User Restore(UserId id, UserRole role, FirstName? firstName, LastName? lastName, TelegramId? telegramId)
            => new(id, role, firstName, lastName, telegramId);

        public override String ToString()
            => $"{nameof(Id)}: {Id} \t {nameof(Role)}: {Role}";

        public override Boolean Equals(Object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj is User other && Id == other.Id;
        }

        public override Int32 GetHashCode() => Id.GetHashCode();
    }
}
