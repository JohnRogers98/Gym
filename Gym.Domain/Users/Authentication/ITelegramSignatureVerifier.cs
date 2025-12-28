namespace Gym.Domain.Users.Authentication
{
    public interface ITelegramSignatureVerifier
    {
        Result<ValidatedTelegramUserInfo> Verify(String rawInitData);
    }
}
