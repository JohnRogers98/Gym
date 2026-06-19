using Ardalis.ApiEndpoints;
using Gym.AuthorizationServer.Admin.Extensions;
using Gym.AuthorizationServer.Admin.Extensions.Mappings;
using Gym.WebDto.Requests.Roles;
using Gym.WebDto.Responses.Roles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.AuthorizationServer.Admin.Controllers.Api.Roles
{
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.AdminOnly))]
    public class CreateUserRoleEndpoint(IMediator _mediator) : EndpointBaseAsync.WithRequest<CreateUserRoleRequest>.WithActionResult<CreateUserRoleResponse>
    {
        [HttpPost("api/user-roles")]
        public async override Task<ActionResult<CreateUserRoleResponse>> HandleAsync(CreateUserRoleRequest request, CancellationToken cancellationToken = default)
        {
            var createUserRoleResult = await _mediator.Send(request.ToApplicationRequest(), cancellationToken);

            if (createUserRoleResult.IsSuccess)
            {
                CreateUserRoleResponse response = new() { RoleId = createUserRoleResult.Value.Id };
                return base.Ok(createUserRoleResult.Value);
            }   

            return createUserRoleResult.ErrorCode switch
            {
                "role_exists" => this.ConflictProblem(createUserRoleResult.ErrorDescription),
                _ => this.InternalErrorProblem(createUserRoleResult.ErrorDescription)
            };
        }
    }
}
