using Gym.Domain._Common;

namespace Gym.Domain.UserContext.Authentication
{
    public interface ITelegramSignatureVerifier
    {
        Result<ValidatedTelegramUserInfo> Verify(String rawInitData);
    }
}
