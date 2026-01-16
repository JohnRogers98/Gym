using Gym.Domain._Shared;
using Gym.Domain.UserAggregate;
using Gym.Infrastructure.Entities.Repositories.Users;

namespace Gym.Infrastructure.Entities.Extensions.Mappings
{
    internal static class UserExtensions
    {
        public static User ToDomain(this UserEntity entity)
        {
            var isParsed = Enum.TryParse<UserRole>(entity.Role, true, out UserRole userRole);
            if (!isParsed)
            {
                throw new ArgumentException($"Failed to parse role for user {entity.Id}");
            }

            return User.Restore(UserId.From(entity.Id.ToString()), userRole, TelegramId.From(entity.TelegramId ?? default));
        }

        public static UserEntity ToEntity(this User user)
        {
            return new() { Id = user.Id.Value.ToObjectId(), Role = user.Role.ToString(), TelegramId = user.TelegramId?.Value };
        }
    }
}
