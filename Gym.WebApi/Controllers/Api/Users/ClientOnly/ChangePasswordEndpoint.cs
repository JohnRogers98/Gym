using Ardalis.ApiEndpoints;
using Gym.Application.Services.UserApi.ChangePassword;
using Gym.Domain._Shared.Errors;
using Gym.Domain.ClientContext.Errors;
using Gym.Domain.FormAuthContext.Errors;
using Gym.WebApi.Extensions;
using Gym.WebDto.Requests.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.Users.ClientOnly
{
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.ClientOnly))]
    public class ChangePasswordEndpoint(IMediator _mediator) : EndpointBaseAsync
        .WithRequest<ChangePasswordRequest>
        .WithActionResult
    {
        [HttpPost("api/users/actions/change-password")]
        public override async Task<ActionResult> HandleAsync(ChangePasswordRequest request, CancellationToken cancellationToken = default)
        {
            ChangePassword changePassword = new(User.GetRequiredUserId(), request.OldPassword, request.NewPassword); 

            var changePasswordResult = await _mediator.Send(changePassword);

            if (changePasswordResult.Success)
            {
                return base.Ok();
            }

            return changePasswordResult.Error switch
            {
                PasswordHashMatchError => this.ConflictProblem(changePasswordResult.Error!.GetErrorMessage()),

                UserIdValidationError or ClientNotFoundByUserIdError or _ => this.InternalErrorProblem(changePasswordResult.Error!.GetErrorMessage())
            };
        }
    }
}
