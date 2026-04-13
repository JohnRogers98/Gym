using Gym.Application.Extensions;
using Gym.Domain._Shared;
using Gym.Domain.FormAuthContext;
using Gym.Domain.FormAuthContext.ValueObjects;
using Gym.Infrastructure.Entities.Repositories.FormAuths;

namespace Gym.Infrastructure.Entities.Extensions.Mappings
{
    internal static class FormAuthExtensions
    {
        public static FormAuth ToDomain(this FormAuthEntity entity)
        {
            return FormAuth.Restore(
                 Login.From(entity.Login).Unwrap(),
                 HashedPassword.From(entity.Password).Unwrap(),
                 UserId.From(entity.UserId.ToString()).Unwrap()
            );
        }

        public static FormAuthEntity ToEntity(this FormAuth formAuth)
        {
            return new()
            {
                Login = formAuth.Login.Value,
                Password = formAuth.Password.Value,
                UserId = formAuth.UserId.Value.ToObjectId()
            };
        }
    }
}
