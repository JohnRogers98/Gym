using AutoMapper;
using Gym.Abstractions.Query.Clients;
using Gym.Application.Services.ClientApi.GetClientById;
using Gym.WebApi.Extensions;
using Gym.WebDto.Responses.Clients;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.Clients.ClientOnly
{
    [Route("api/clients")]
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.ClientOnly))]
    public class GetClientController(IMediator _mediator, IMapper _mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<GetClientResponse>> GetClient()
        {
            GetClientById getClientById = new(User.GetRequiredClientId());
            ClientProjection clientProjection = await _mediator.Send(getClientById);

            return base.Ok(_mapper.Map<GetClientResponse>(clientProjection));
        }
    }
}
