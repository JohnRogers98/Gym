using Gym.Application.Services.UserApi.TelegramAuthentication;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Gym.WebApi.Controllers.Api.Users.Jwt
{
    public interface IAccessTokenGenerator
    {
        String Generate(AuthenticatedUserDetails authenticatedUserDetails);
    }

    public class AccessTokenGenerator(IConfiguration _configuration) : IAccessTokenGenerator
    {
        private SymmetricSecurityKey Key => field ??= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT_SECRET"]!));

        public String Generate(AuthenticatedUserDetails authenticatedUserDetails)
        {
            var claimsIdentity = new ClaimsIdentity([
                new Claim(ClaimTypes.NameIdentifier, authenticatedUserDetails.UserId),
                new Claim(JwtRegisteredClaimNames.Sub, authenticatedUserDetails.UserId),
                new Claim(ClaimTypes.Role, authenticatedUserDetails.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("ClientId", authenticatedUserDetails.ClientId)
                ]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddDays(1),
                Issuer = "Gym.WebApi",
                Audience = "Gym.WebApplication",
                SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256)
            };

            return new JsonWebTokenHandler().CreateToken(tokenDescriptor);
        }
    }
}
