using Ardalis.ApiEndpoints;
using AutoMapper;
using Gym.Application.Services.ClientApi.CreateClient;
using Gym.Domain._Shared.Errors;
using Gym.Domain.FormAuthContext.Errors;
using Gym.Domain.UserContext.Errors;
using Gym.WebApi.Extensions;
using Gym.WebDto.Requests.Client;
using Gym.WebDto.Responses.Clients;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.Clients.AdminOnly
{
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.AdminOnly))]
    public class CreateClientEndpoint(IMediator _mediator, IMapper _mapper) : EndpointBaseAsync
        .WithRequest<CreateClientRequest>
        .WithActionResult<CreateClientResponse>
    {
        [HttpPost("api/clients")]
        public override async Task<ActionResult<CreateClientResponse>> HandleAsync(CreateClientRequest request, CancellationToken cancellationToken = default)
        {
            var createClient = _mapper.Map<CreateClient>(request);
            var createClientResult = await _mediator.Send(createClient);

            if (createClientResult.Success)
            {
                return Accepted(
                  $"api/clients/{createClientResult.Data!.UserId}",
                  value: _mapper.Map<CreateClientResponse>(createClientResult.Data!));
            }

            return createClientResult.Error switch
            {
                LoginValidationError 
                or UserRoleParseError 
                or FirstNameValidationError 
                or LastNameValidationError => this.BadRequestProblem(createClientResult.Error!.GetErrorMessage()),

                LoginAlreadyExistsError => this.ConflictProblem(createClientResult.Error!.GetErrorMessage()),

                _ => this.InternalErrorProblem(createClientResult.Error!.GetErrorMessage())
            };
        }
    }
}
