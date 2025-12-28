namespace Gym.Domain.UserAggregate.Authentication
{
    public interface ITelegramSignatureVerifier
    {
        Result<ValidatedTelegramUserInfo> Verify(String rawInitData);
    }
}
