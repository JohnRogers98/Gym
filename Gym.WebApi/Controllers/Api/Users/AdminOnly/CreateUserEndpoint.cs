using Ardalis.ApiEndpoints;
using AutoMapper;
using Gym.Application.Services.UserApi.CreateUser;
using Gym.Domain._Shared.Errors;
using Gym.Domain.FormAuthContext.Errors;
using Gym.Domain.UserContext.Errors;
using Gym.WebApi.Extensions;
using Gym.WebDto.Requests.Users;
using Gym.WebDto.Responses.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.Users.AdminOnly
{
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.AdminOnly))]
    public class CreateUserEndpoint(IMediator _mediator, IMapper _mapper) : EndpointBaseAsync
        .WithRequest<CreateUserRequest>
        .WithActionResult<CreateUserResponse>
    {
        [HttpPost("api/users")]
        public override async Task<ActionResult<CreateUserResponse>> HandleAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
        {
            var createUser = _mapper.Map<CreateUser>(request);
            var createUserResult = await _mediator.Send(createUser);

            if (createUserResult.Success)
            {
                return base.Ok(_mapper.Map<CreateUserResponse>(createUserResult.Data!));
            }

            return createUserResult.Error switch
            {
                LoginValidationError 
                or UserRoleParseError 
                or FirstNameValidationError 
                or LastNameValidationError => this.BadRequestProblem(createUserResult.Error!.GetErrorMessage()),

                LoginAlreadyExistsError => this.ConflictProblem(createUserResult.Error!.GetErrorMessage()),

                _ => this.InternalErrorProblem(createUserResult.Error!.GetErrorMessage())
            };
        }
    }
}
