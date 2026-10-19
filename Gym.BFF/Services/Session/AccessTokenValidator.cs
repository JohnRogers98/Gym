using Gym.Redis.Client.Services.Ports;
using System.IdentityModel.Tokens.Jwt;

namespace Gym.BFF.Services.Session
{
    public class AccessTokenValidator : IAccessTokenValidator
    {
        private readonly JwtSecurityTokenHandler _jwtHandler = new JwtSecurityTokenHandler();

        public Boolean Validate(String accessToken)
        {
            var jwtToken = _jwtHandler.ReadJwtToken(accessToken);
            return jwtToken.ValidTo > DateTime.UtcNow;
        }
    }
}
