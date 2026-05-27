using Gym.AuthorizationServer.Shared.Abstractions;

namespace Gym.AuthorizationServer.Services.Flows
{
    public interface ITokenFlowCoordinator
    {
        Task<Result<AuthorizationCodeResponse>> AuthorizationCodeAsync(AuthorizationCodeRequest request, CancellationToken cancellationToken);
        Task<Result<RefreshTokenResponse>> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken);
        Task<Result<TelegramAssertionResponse>> TelegramAssertionAsync(TelegramAssertionRequest request, CancellationToken cancellationToken);
    }

    public class TokenFlowCoordinator(
        IAuthorizationCodeFlowService _authorizationCodeFlowService,
        IRefreshTokenFlowService _refreshTokenFlowService,
        ITelegramAssertionFlowService _telegramAssertionFlowService) : ITokenFlowCoordinator
    {
        public async Task<Result<AuthorizationCodeResponse>> AuthorizationCodeAsync(AuthorizationCodeRequest request, CancellationToken cancellationToken)
        {
            return await _authorizationCodeFlowService.HandleAsync(request, cancellationToken);
        }

        public async Task<Result<RefreshTokenResponse>> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            return await _refreshTokenFlowService.HandleAsync(request, cancellationToken);
        }

        public async Task<Result<TelegramAssertionResponse>> TelegramAssertionAsync(TelegramAssertionRequest request, CancellationToken cancellationToken)
        {
            return await _telegramAssertionFlowService.HandleAsync(request, cancellationToken);
        }
    }
}
