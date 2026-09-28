using Gym.Redis.Client.Services.Ports;
using Microsoft.Extensions.Options;
using RedLockNet.SERedis;

namespace Gym.Redis.Client.Services
{
    public interface IRefreshSessionTokensService
    {
        Task<SessionTokens?> HandleAsync(String clientSessionKey, SessionTokens oldSessionTokens, CancellationToken cancellationToken);
    }

    internal class RefreshSessionTokensService(
        IExchangeRefreshTokenService _exchangeRefreshTokenService,
        RedLockFactory _redLockFactory,
        IOptions<RedisOptions> _redisOptions,
        IGetSessionTokensService _getSessionTokensService,
        ISaveSessionTokensService _saveSessionTokensService) : IRefreshSessionTokensService
    {
        public async Task<SessionTokens?> HandleAsync(String clientSessionKey, SessionTokens oldSessionTokens, CancellationToken cancellationToken)
        {
            await using (var redLock = await _redLockFactory.CreateLockAsync(
                clientSessionKey,
                _redisOptions.Value.LockKeyExpiry,
                _redisOptions.Value.LockWait,
                _redisOptions.Value.LockRetry))
            {
                if (redLock.IsAcquired)
                {
                    // Double-check
                    var latestTokens = await _getSessionTokensService.HandleAsync(clientSessionKey);
                    if (await this.WasTokenRefreshedAsync(oldSessionTokens.AccessToken, latestTokens!.AccessToken))
                    {
                        return latestTokens;
                    }

                    var newSessionTokens = await _exchangeRefreshTokenService.HandleAsync(oldSessionTokens.RefreshToken!, cancellationToken);
                    if(newSessionTokens is not null)
                    {
                        await _saveSessionTokensService.HandleAsync(clientSessionKey, newSessionTokens);
                        return newSessionTokens;
                    }
                }

                return null;
            }
        }

        private async Task<Boolean> WasTokenRefreshedAsync(String originalAccessToken, String latestAccessToken) =>
            originalAccessToken != latestAccessToken;

    }
}
