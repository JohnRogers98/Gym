using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.FormAuthContext.ValueObjects;

namespace Gym.Domain.FormAuthContext
{
    public class FormAuth : AggregateRoot
    {
        public Login Login { get; }
        public HashedPassword Password { get; private set; }
        public UserId UserId { get; }

        private FormAuth(Login id, HashedPassword password, UserId userId)
        {
            Login = id;
            Password = password;
            UserId = userId;
        }

        public static FormAuth Create(Login id, HashedPassword password, UserId userId) 
            => new(id, password, userId);

        public static FormAuth Restore(Login id, HashedPassword password, UserId userId)
            => new(id, password, userId);

        public void ChangePassword(HashedPassword newPassword)
            => this.Password = newPassword;

        public override String ToString()
            => $"{nameof(UserId)}: {UserId} \t {nameof(Login)}: {Login.Value}";

        public override Boolean Equals(Object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj is FormAuth other && Login == other.Login;
        }

        public override Int32 GetHashCode() => Login.GetHashCode();
    }
}
