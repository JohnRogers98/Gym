using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Gym.WebApi.Controllers.Api.Users.Jwt
{
    public interface IAccessTokenGenerator
    {
        String Generate(String userId);
    }

    public class AccessTokenGenerator(IConfiguration _configuration) : IAccessTokenGenerator
    {
        private SymmetricSecurityKey Key => field ??= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT_SECRET"]!));

        public String Generate(String userId)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(JwtRegisteredClaimNames.Sub, userId), new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) }),
                Expires = DateTime.UtcNow.AddDays(1),
                Issuer = "Gym.WebApi",
                Audience = "Gym.WebApplication",
                SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256)
            };

            return new JsonWebTokenHandler().CreateToken(tokenDescriptor);
        }
    }
}
