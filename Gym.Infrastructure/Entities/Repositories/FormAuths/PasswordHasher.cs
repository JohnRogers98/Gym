using Gym.Application.Extensions;
using Gym.Domain.FormAuthContext;
using Gym.Domain.FormAuthContext.ValueObjects;
using Isopoh.Cryptography.Argon2;

namespace Gym.Infrastructure.Entities.Repositories.FormAuths
{
    internal class PasswordHasher : IPasswordHasher
    {
        public HashedPassword HashPassword(Password password) 
            => HashedPassword.From(Argon2.Hash(password.Value)).Unwrap();
    }
}
