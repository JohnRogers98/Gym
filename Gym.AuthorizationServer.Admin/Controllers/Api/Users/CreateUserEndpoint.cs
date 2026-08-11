using Ardalis.ApiEndpoints;
using Gym.AuthorizationServer.Admin.Extensions;
using Gym.AuthorizationServer.Admin.Extensions.Mappings;
using Gym.WebDto.Requests.Users;
using Gym.WebDto.Responses.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.AuthorizationServer.Admin.Controllers.Api.Users
{
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.AdminOnly))]
    public class CreateUserEndpoint(IMediator _mediator) : EndpointBaseAsync.WithRequest<CreateUserRequest>.WithActionResult<CreateUserResponse>
    {
        [HttpPost("api/users")]
        public async override Task<ActionResult<CreateUserResponse>> HandleAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
        {
            var createUserResult = await _mediator.Send(request.ToApplicationRequest(), cancellationToken);

            if (createUserResult.IsSuccess)
            {
                var response = new CreateUserResponse() { UserId = createUserResult.Value.UserId };
                return base.Ok(response);
            }

            return createUserResult.ErrorCode switch
            {
                "invalid_request" => this.BadRequestProblem(createUserResult.ErrorDescription),
                "username_exists" => this.ConflictProblem(createUserResult.ErrorDescription),
                _ => this.InternalErrorProblem(createUserResult.ErrorDescription)
            };
        }
    }
}
