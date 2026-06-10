using Gym.AuthorizationServer.Client.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Gym.BFF.Services
{
    public interface IOAuthIdTokenValidator
    {
        Task<Result<ClaimsPrincipal>> ValidateAsync(String idToken, String? accessToken, String? nonce, CancellationToken cancellationToken);
    }

    public class OAuthIdTokenValidator(
        ClientCredentialsOptions _clientCredentialsOptions,
        AuthorizationServerOptions _authorizationServerOptions,
        IRsaSecurityKeyProvider _rsaSecurityKeyProvider,
        IComputeOpenIdAtHashService _computeOpenIdAtHashService) : IOAuthIdTokenValidator
    {
        private static readonly JwtSecurityTokenHandler _tokenHandler = new();

        public async Task<Result<ClaimsPrincipal>> ValidateAsync(String idToken, String? accessToken, String? nonce, CancellationToken cancellationToken)
        {
            var kid = _tokenHandler.ReadJwtToken(idToken).Header.Kid;

            TokenValidationParameters tokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidIssuer = _authorizationServerOptions.BaseUrl,

                ValidateAudience = true,
                ValidAudience = _clientCredentialsOptions.ClientId,

                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(1),

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = await _rsaSecurityKeyProvider.GetKeyAsync(kid, cancellationToken),

                ValidTypes = ["id_token+jwt"],

                ValidAlgorithms = new[] { SecurityAlgorithms.RsaSha256 }
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
