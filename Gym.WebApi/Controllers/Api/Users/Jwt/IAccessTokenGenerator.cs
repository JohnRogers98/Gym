using Gym.Application.Services.UserApi;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Gym.WebApi.Controllers.Api.Users.Jwt
{
    public interface IAccessTokenGenerator
    {
        String Generate(UserDetails userDetails);
    }

    public class AccessTokenGenerator(IConfiguration _configuration) : IAccessTokenGenerator
    {
        private SymmetricSecurityKey Key => field ??= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT_SECRET"]!));

        public String Generate(UserDetails userDetails)
        {
            var claimsIdentity = new ClaimsIdentity([
                new Claim(JwtRegisteredClaimNames.Sub, userDetails.id),
                new Claim(ClaimTypes.Role, userDetails.role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
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
