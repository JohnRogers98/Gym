namespace Gym.Redis.Client.Services.Ports
{
    public interface IExchangeRefreshTokenService
    {
        Task<SessionTokens?> HandleAsync(String refreshToken, CancellationToken cancellationToken);
    }
}
