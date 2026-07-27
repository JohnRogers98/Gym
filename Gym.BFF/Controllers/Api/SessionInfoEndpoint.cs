using Gym.AuthorizationServer.Client.Services;
using Gym.BFF.Options;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json.Serialization;

namespace Gym.BFF.Controllers.Api
{
    [ApiController]
    public class SessionInfoEndpoint(IGetUserInfoService _getUserInfoService) : ControllerBase
    {
        [HttpGet("api/session-info")]
        public async Task<ActionResult<SessionInfo>> HandleAsync(CancellationToken cancellationToken)
        {
            var accessToken = User.FindFirst(ExtendedClaimTypes.AccessToken)?.Value;
            if (String.IsNullOrEmpty(accessToken))
                return Unauthorized();

            var userInfoResult = await _getUserInfoService.HandleAsync(accessToken, cancellationToken);
            if(userInfoResult.IsFailure)
                return BadRequest(userInfoResult.Error);

            SessionInfo sessionInfo = new()
            {
                UserId = userInfoResult.Value.Subject,
                Name = userInfoResult.Value.Name,
                GivenName = userInfoResult.Value.GivenName,
                FamilyName = userInfoResult.Value.FamilyName,
                Role = userInfoResult.Value.Role
            };

            var jwtIdToken = base.HttpContext.User.FindFirst(ExtendedClaimTypes.IdToken)?.Value;
            if (jwtIdToken is not null)
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(jwtIdToken);
                var acr = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Acr)?.Value;
                sessionInfo.AuthenticationMethod = acr;
            }

            return Ok(sessionInfo);
        }
    }

    public class SessionInfo
    {
        public required String UserId { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public String? Name { get; set; }
 
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public String? GivenName { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public String? FamilyName { get; set; }

        public String? Role { get; set; }

        public String? AuthenticationMethod { get; set; }
    }
}
