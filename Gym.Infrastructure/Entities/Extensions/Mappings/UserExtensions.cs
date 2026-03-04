using Gym.Domain._Shared;
using Gym.Domain.UserContext;
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

            return User.Restore(
                UserId.From(entity.Id.ToString()),
                userRole, 
                TelegramId.From(entity.TelegramId ?? default),
                entity.TelegramUsername is not null ? TelegramUsername.From(entity.TelegramUsername) : null, 
                entity.FirstName is not null ? FirstName.From(entity.FirstName) : null, 
                entity.LastName is not null ? LastName.From(entity.LastName) : null
            );
        }

        public static UserEntity ToEntity(this User user)
        {
            return new() 
            { 
                Id = user.Id.Value.ToObjectId(),
                Role = user.Role.ToString(),
                TelegramId = user.TelegramId?.Value,
                TelegramUsername = user.TelegramUsername?.Value,
                FirstName = user.FirstName?.Value,
                LastName = user.LastName?.Value,
            };
        }
    }
}
