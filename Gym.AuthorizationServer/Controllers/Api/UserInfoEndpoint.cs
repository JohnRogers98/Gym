using Ardalis.ApiEndpoints;
using Gym.AuthorizationServer.Extensions;
using Gym.AuthorizationServer.Infrastructure.Entities.Users;
using Gym.OAuth.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.AuthorizationServer.Controllers.Api
{
    [Route("userinfo")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class UserInfoEndpoint(IUserRepository _userRepository) : EndpointBaseAsync.WithoutRequest.WithActionResult<UserInfo>
    {
        [HttpGet, HttpPost]
        public override async Task<ActionResult<UserInfo>> HandleAsync(CancellationToken cancellationToken = default)
        {
            if (String.IsNullOrEmpty(User.GetSub()))
                return base.Unauthorized(new { error = "invalid_token", error_description = "Missing 'sub' claim" });

            var scopes = User.GetScope()?.Split(' ') ?? Array.Empty<String>();

            
            if (!scopes.Contains("openid"))
                return base.Forbid();

            var user = await _userRepository.GetByIdAsync(User.GetSub()!, cancellationToken);
            if (user is null)
                return base.Unauthorized(new { error = "invalid_token", error_description = "User not found" });

            UserInfo userInfo = new()
            {
                Subject = user.Id
            };

            if (scopes.Contains("profile"))
            {
                userInfo.Name = $"{user.FirstName} {user.LastName}";
                userInfo.GivenName = user.FirstName;
                userInfo.FamilyName = user.LastName;
            }
            if (scopes.Contains("email")) { }
            if (scopes.Contains("phone")) { }

            return base.Ok(userInfo);
        }
    }
}
