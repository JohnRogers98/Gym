using Gym.AuthorizationServer.Client.Services;
using Gym.Redis.Client;
using Gym.Redis.Client.Services.Ports;

namespace Gym.BFF.Services.Session
{
    public class ExchangeRefreshTokenService(IRefreshTokenService _refreshTokenService) : IExchangeRefreshTokenService
    {
        public async Task<SessionTokens?> HandleAsync(String refreshToken, CancellationToken cancellationToken)
        {
            var newTokensResponseResult = await _refreshTokenService.HandleAsync(refreshToken, cancellationToken: cancellationToken);
            if (newTokensResponseResult.IsSuccess)
            {
                return new SessionTokens
                {
                    AccessToken = newTokensResponseResult.Value.AccessToken,
                    RefreshToken = newTokensResponseResult.Value.RefreshToken,
                    IdToken = newTokensResponseResult.Value.IdToken
                };
            }

            return null;
        }
    }
}
