using Gym.BFF.Options;
using Gym.BFF.Services.Jwks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Gym.BFF.Services.Token
{
    public interface IOAuthIdTokenValidator
    {
        Task<Result<ClaimsPrincipal>> ValidateAsync(String idToken, String? accessToken, String? nonce, CancellationToken cancellationToken);
    }

    public class OAuthIdTokenValidator(
        IOptions<ClientCredentialsOptions> _clientCredentials,
        IOptions<UrlsOptions> _urls,
        IRsaSecurityKeyProvider _rsaSecurityKeyProvider,
        IComputeOpenIdAtHashService _computeOpenIdAtHashService) : IOAuthIdTokenValidator
    {
        private static readonly JwtSecurityTokenHandler _tokenHandler = new();

        public async Task<Result<ClaimsPrincipal>> ValidateAsync(String idToken, String? accessToken, String? nonce, CancellationToken cancellationToken)
        {
            TokenValidationParameters tokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidIssuer = _urls.Value.AuthorizationServer.BaseUrl,

                ValidateAudience = true,
                ValidAudience = _clientCredentials.Value.ClientId,

                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(1),

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = await _rsaSecurityKeyProvider.GetKeyAsync(cancellationToken)
            };

            ClaimsPrincipal claimsPrincipal;
            try
            {
                claimsPrincipal = _tokenHandler.ValidateToken(idToken, tokenValidationParameters, out _);
            }
            catch
            {
                return Result<ClaimsPrincipal>.Failure("invalid_token", "Token validation failed");
            }

            if (nonce is not null && claimsPrincipal.GetNonce() != nonce)    
                return Result<ClaimsPrincipal>.Failure("invalid_nonce", "Nonce mismatch");

            if (accessToken is not null && _computeOpenIdAtHashService.Compute(accessToken) != claimsPrincipal.GetAtHash())
                return Result<ClaimsPrincipal>.Failure("invalid_atHash", "AtHash mismatch");

            return Result<ClaimsPrincipal>.Success(claimsPrincipal);
        }
    }
}
