using Gym.AuthorizationServer.Infrastructure.Entities.UserConsents;
using Gym.AuthorizationServer.Options;
using Gym.AuthorizationServer.Services.Rsa;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Gym.AuthorizationServer.Services.Tokens
{
    public interface IAccessTokenGenerator
    {
        String GenerateToken(UserConsentEntity userConsent);
    }

    public class AccessTokenGenerator(IRsaSigningCredentialsProvider _rsaSigningService, IOptions<JwtOptions> _jwtOptions) : IAccessTokenGenerator
    {
        public const String TypHeader = "at+JWT";

        public String GenerateToken(UserConsentEntity userConsent)
        {
            var claimsIdentity = new ClaimsIdentity([
                new Claim(JwtRegisteredClaimNames.Iss, _jwtOptions.Value.Issuer),
                new Claim(JwtRegisteredClaimNames.Sub, userConsent.UserId),
                new Claim(JwtRegisteredClaimNames.Aud, userConsent.ClientId),
                //new Claim(ClaimTypes.Role, String.Join(' ', userConsent.GrantedScopes))
                ]);

            if (userConsent.GrantedScopes is not null && userConsent.GrantedScopes.Any())
                claimsIdentity.AddClaim(new Claim("scope", String.Join(' ', userConsent.GrantedScopes.Select(aScope => aScope.Name))));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = _rsaSigningService.GetSigningCredentials(),
                TokenType = TypHeader
            };

            return new JsonWebTokenHandler().CreateToken(tokenDescriptor);
        }
    }
}
