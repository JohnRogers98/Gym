using Gym.BFF.Options;
using Gym.BFF.Services;
using Gym.OAuth.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Gym.BFF.Controllers.Api
{
    [ApiController]
    [Route("api/userinfo")]
    public class UserInfoEndpoint(IGetUserInfoService _getUserInfoService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<UserInfo>> GetUserInfo(CancellationToken cancellationToken)
        {
            var accessToken = User.FindFirst(ExtendedClaimTypes.AccessToken)?.Value;

            if (string.IsNullOrEmpty(accessToken))
                return Unauthorized();

            var userInfoResult = await _getUserInfoService.HandleAsync(accessToken, cancellationToken);
            if(userInfoResult.IsFailure)
                return BadRequest(userInfoResult.Error);

            return Ok(userInfoResult.Value);
        }
    }
}
