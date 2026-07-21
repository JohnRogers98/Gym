using Ardalis.ApiEndpoints;
using Gym.AuthorizationServer.Admin.Application.Services.UserApi.CheckUsernameExistence;
using Gym.AuthorizationServer.Admin.Extensions;
using Gym.WebDto.Requests.Users;
using Gym.WebDto.Responses.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.AuthorizationServer.Admin.Controllers.Api.Users
{
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.AdminOnly))]
    public class CheckUsernameExistingEndpoint(IMediator _mediator) : EndpointBaseAsync.WithRequest<CheckUsernameExistenceRequest>.WithActionResult<CheckUsernameExistenceResponse>
    {
        [HttpPost("api/users/check-username")]
        public async override Task<ActionResult<CheckUsernameExistenceResponse>> HandleAsync(CheckUsernameExistenceRequest request, CancellationToken cancellationToken = default)
        {
            var usernameExistResult = await _mediator.Send(new CheckUsernameExistence(request.Username), cancellationToken);

            if (usernameExistResult.IsSuccess)
                return base.Ok(new CheckUsernameExistenceResponse { IsExist = usernameExistResult.Value });

            else
                return this.InternalErrorProblem(usernameExistResult.ErrorDescription);
        }
    }
}
