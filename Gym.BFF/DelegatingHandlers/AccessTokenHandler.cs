using Gym.BFF.Options;
using Gym.Redis.Client;
using Gym.Redis.Client.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;

namespace Gym.BFF.DelegatingHandlers;

public class AccessTokenHandler(
    IHttpContextAccessor _httpContextAccessor,
    IGetSessionTokensService _getSessionTokens,
    IRefreshSessionTokensService _refreshSessionTokensService) : DelegatingHandler
{
    JwtSecurityTokenHandler _jwtHandler = new JwtSecurityTokenHandler();

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

        SessionTokens? sessionTokens = await _getSessionTokens.HandleAsync(clientSessionKey);
        if (String.IsNullOrWhiteSpace(sessionTokens?.AccessToken))
            return;

        if (this.IsTokenValid(sessionTokens.AccessToken))
        {
            this.AppendAuthorizationHeader(request, sessionTokens.AccessToken);
        }
        else
        {
            SessionTokens? refreshedSessionTokens = await _refreshSessionTokensService
                .HandleAsync(clientSessionKey, sessionTokens, cancellationToken);
            if (refreshedSessionTokens is not null)
            {
                this.AppendAuthorizationHeader(request, refreshedSessionTokens.AccessToken);
            }
        }
    }
    private Boolean IsTokenValid(String accessToken)
    {
        var jwtToken = _jwtHandler.ReadJwtToken(accessToken);
        return jwtToken.ValidTo > DateTime.UtcNow;
    }
    private void AppendAuthorizationHeader(HttpRequestMessage request, String accessToken)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
    }

}
