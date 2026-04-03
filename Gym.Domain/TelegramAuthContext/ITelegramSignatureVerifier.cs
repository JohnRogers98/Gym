using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.TelegramAuthContext.ValueObjects;

namespace Gym.Domain.TelegramAuthContext
{
    public interface ITelegramSignatureVerifier
    {
        Result<ValidatedTelegramUserInfo> Verify(String rawInitData);
    }

    public record ValidatedTelegramUserInfo
    {
        public TelegramId Id { get; }
        public TelegramUsername? Username { get; }
        public FirstName? FirstName { get; }
        public LastName? LastName { get; }

        private ValidatedTelegramUserInfo(TelegramId id, TelegramUsername? username, FirstName? firstName, LastName? lastName)
        {
            Id = id;
            Username = username;
            FirstName = firstName;
            LastName = lastName;
        }

        public static ValidatedTelegramUserInfo From(TelegramId id, TelegramUsername? username = null, FirstName? firstName = null, LastName? lastName = null)
            => new(id, username, firstName, lastName);
    }
}
