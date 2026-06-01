using Ardalis.ApiEndpoints;
using Gym.AuthorizationServer.Extensions;
using Gym.AuthorizationServer.Infrastructure.Entities.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Gym.AuthorizationServer.Controllers.Api
{
    [Route("userinfo")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class UserInfoEndpoint(IUserRepository _userRepository) : EndpointBaseAsync.WithoutRequest.WithActionResult<UserInfoResponse>
    {
        [HttpGet, HttpPost]
        public override async Task<ActionResult<UserInfoResponse>> HandleAsync(CancellationToken cancellationToken = default)
        {
            if (String.IsNullOrEmpty(User.GetSub()))
                return base.Unauthorized(new { error = "invalid_token", error_description = "Missing 'sub' claim" });

            var scopes = User.GetScope()?.Split(' ') ?? Array.Empty<String>();

            
            if (!scopes.Contains("openid"))
                return base.Forbid();

            var user = await _userRepository.GetByIdAsync(User.GetSub()!, cancellationToken);
            if (user is null)
                return base.Unauthorized(new { error = "invalid_token", error_description = "User not found" });

            UserInfoResponse userInfo = new()
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

    public class UserInfoResponse
    {
        [JsonPropertyName("sub")]
        public required String Subject { get; set; }

        #region profile
        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public String? Name { get; set; }

        [JsonPropertyName("given_name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public String? GivenName { get; set; }

        [JsonPropertyName("family_name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public String? FamilyName { get; set; }
        #endregion
    }
}
