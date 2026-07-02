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
        String GenerateToken(AccessTokenClaimsMetadata accessTokenClaimsMetadata);
    }

    public class AccessTokenGenerator(IRsaSigningCredentialsProvider _rsaSigningService, IOptions<JwtOptions> _jwtOptions) : IAccessTokenGenerator
    {
        public const String TypHeader = "at+JWT";

        public String GenerateToken(AccessTokenClaimsMetadata accessTokenClaimsMetadata)
        {
            var claimsIdentity = new ClaimsIdentity([
                new Claim(JwtRegisteredClaimNames.Iss, _jwtOptions.Value.Issuer),
                new Claim(JwtRegisteredClaimNames.Aud, _jwtOptions.Value.Audience),
                new Claim(JwtRegisteredClaimNames.Sub, accessTokenClaimsMetadata.UserId)
            ]);

            if(!String.IsNullOrEmpty(accessTokenClaimsMetadata.UserRole))
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, accessTokenClaimsMetadata.UserRole));
            }

            if (accessTokenClaimsMetadata.GrantedScopes is not null && accessTokenClaimsMetadata.GrantedScopes.Any())
                claimsIdentity.AddClaim(new Claim("scope", String.Join(' ', accessTokenClaimsMetadata.GrantedScopes.Select(aScope => aScope.Name))));

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

    public record AccessTokenClaimsMetadata
    {
        public required String UserId { get; init; }
        public String? UserRole { get; init; }
        
        public required String ClientId { get; init; }

        public required ICollection<ScopeInfo> GrantedScopes { get; init; }
    }
}
