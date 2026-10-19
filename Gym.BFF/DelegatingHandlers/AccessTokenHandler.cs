using Gym.BFF.Options;
using Gym.Redis.Client;
using Gym.Redis.Client.Services;
using System.Net.Http.Headers;

namespace Gym.BFF.DelegatingHandlers;

public class AccessTokenHandler(
    IHttpContextAccessor _httpContextAccessor,
    IGetValidSessionTokensService _getValidSessionTokensService) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        await this.AddAccessTokenAsync(request, cancellationToken);

        var response = await base.SendAsync(request, cancellationToken);
        
        return response;
    }

    private async Task AddAccessTokenAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var clientSessionKey = _httpContextAccessor.HttpContext?.User.FindFirst(ExtendedClaimTypes.ClientSessionKey)?.Value;
        if (clientSessionKey is null)
            return;

        if (_httpContextAccessor.HttpContext?.Request.Headers.Authorization.Count > 0)
            return;

        SessionTokens? sessionTokens = await _getValidSessionTokensService.HandleAsync(clientSessionKey, cancellationToken);
        if (String.IsNullOrWhiteSpace(sessionTokens?.AccessToken))
            return;

        this.AppendAuthorizationHeader(request, sessionTokens.AccessToken);
    }

    private void AppendAuthorizationHeader(HttpRequestMessage request, String accessToken)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
    }

}
