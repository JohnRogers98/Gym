using Gym.AuthorizationServer.Entities.UserConsents;
using Gym.AuthorizationServer.Services.Rsa;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Gym.AuthorizationServer.Services
{
    public interface IAccessTokenGenerator
    {
        Task<String> GenerateTokenAsync(UserConsentEntity userConsent, CancellationToken cancellationToken);
    }

    public class AccessTokenGenerator(IRsaSigningService _rsaSigningService) : IAccessTokenGenerator
    {
        public async Task<String> GenerateTokenAsync(UserConsentEntity userConsent, CancellationToken cancellationToken)
        {
            var claimsIdentity = new ClaimsIdentity([
                new Claim(ClaimTypes.NameIdentifier, userConsent.UserId),
                new Claim(JwtRegisteredClaimNames.Sub, userConsent.ClientId),
                new Claim(ClaimTypes.Role, String.Join(' ', userConsent.GrantedScopes))
                ]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddDays(1),
                Issuer = "Gym.AuthorizationServer",
                Audience = userConsent.ClientId,
                SigningCredentials = _rsaSigningService.GetSigningCredentials()
            };

            return new JsonWebTokenHandler().CreateToken(tokenDescriptor);
        }
    }
}
