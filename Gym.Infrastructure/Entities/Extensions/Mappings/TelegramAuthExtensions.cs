using Gym.Application.Extensions;
using Gym.Domain._Shared;
using Gym.Domain.TelegramAuthContext;
using Gym.Domain.TelegramAuthContext.ValueObjects;
using Gym.Infrastructure.Entities.Repositories.TelgramAuths;

namespace Gym.Infrastructure.Entities.Extensions.Mappings
{
    internal static class TelegramAuthExtensions
    {
        public static TelegramAuth ToDomain(this TelegramAuthEntity entity)
        {
            return TelegramAuth.Restore(
                 TelegramId.From(entity.Id).Unwrap(),
                 UserId.From(entity.UserId.ToString()).Unwrap(),   
                 entity.TelegramUsername is not null ? TelegramUsername.From(entity.TelegramUsername).Unwrap() : null
            );
        }

        public static TelegramAuthEntity ToEntity(this TelegramAuth telegramAuth)
        {
            return new()
            {
                Id = telegramAuth.Id.Value,
                UserId = telegramAuth.UserId.Value.ToObjectId(),
                TelegramUsername = telegramAuth.TelegramUsername?.Value
            };
        }
    }
}
