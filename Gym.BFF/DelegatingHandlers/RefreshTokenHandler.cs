using Gym.AuthorizationServer.Client.Services;
using Gym.BFF.Options;
using Gym.BFF.Services.Session;
using System.Net;
using System.Net.Http.Headers;

namespace Gym.BFF.DelegatingHandlers;

public class RefreshTokenHandler(
    IHttpContextAccessor _httpContextAccessor,
    IRefreshTokenService _refreshTokenService,
    ISetTokensToClientSideSessionService _setTokensToClientSideSessionService) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized && _httpContextAccessor.HttpContext != null)
        {
            var refreshToken = _httpContextAccessor.HttpContext.User.FindFirst(ExtendedClaimTypes.RefreshToken)?.Value;

            if (!String.IsNullOrEmpty(refreshToken))
            {
                var newTokensResponseResult = await _refreshTokenService.HandleAsync(refreshToken, cancellationToken: cancellationToken);
                if(newTokensResponseResult.IsSuccess)
                {
                    await _setTokensToClientSideSessionService
                        .HandleAsync(newTokensResponseResult.Value.AccessToken, newTokensResponseResult.Value.RefreshToken, newTokensResponseResult.Value.IdToken);
                    
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newTokensResponseResult.Value.AccessToken);
                    response = await base.SendAsync(request, cancellationToken);
                }
            }
        }

        return response;
    }
}
