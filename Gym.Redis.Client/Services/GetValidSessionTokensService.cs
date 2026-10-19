using Gym.Redis.Client.Services.Ports;

namespace Gym.Redis.Client.Services
{
    public interface IGetValidSessionTokensService
    {
        Task<SessionTokens?> HandleAsync(String clientSessionKey, CancellationToken cancellationToken);
    }

    public class GetValidSessionTokensService(
        IGetSessionTokensService _getSessionTokensService,
        IAccessTokenValidator _accessTokenValidator,
        IRefreshSessionTokensService _refreshSessionTokensService) : IGetValidSessionTokensService
    {
        public async Task<SessionTokens?> HandleAsync(String clientSessionKey, CancellationToken cancellationToken)
        {
            var tokens = await _getSessionTokensService.HandleAsync(clientSessionKey);

            if(tokens is null || _accessTokenValidator.Validate(tokens.AccessToken))
                return tokens;

            return await _refreshSessionTokensService.HandleAsync(clientSessionKey, tokens, cancellationToken);
        }
    }
}
