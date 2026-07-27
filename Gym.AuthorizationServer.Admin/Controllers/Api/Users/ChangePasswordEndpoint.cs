using Ardalis.ApiEndpoints;
using Gym.AuthorizationServer.Admin.Application.Services.UserApi.ChangePassword;
using Gym.AuthorizationServer.Admin.Extensions;
using Gym.WebDto.Requests.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Gym.AuthorizationServer.Admin.Controllers.Api.Users
{
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.AuthenticatedOnly))]
    public class ChangePasswordEndpoint(IMediator _mediator) : EndpointBaseAsync.WithRequest<ChangePasswordRequest>.WithActionResult
    {
        [HttpPost("api/users/change-password")]
        public async override Task<ActionResult> HandleAsync(ChangePasswordRequest request, CancellationToken cancellationToken = default)
        {
            String userId = base.HttpContext.User.GetSub()!;
            ChangePassword changePassword = new(userId, request.CurrentPassword, request.NewPassword);

            var changePasswordResult = await _mediator.Send(changePassword, cancellationToken);
            if (changePasswordResult.IsSuccess)
                return base.NoContent();
            else
                return this.InternalErrorProblem(changePasswordResult.ErrorDescription);
        }
    }
}
