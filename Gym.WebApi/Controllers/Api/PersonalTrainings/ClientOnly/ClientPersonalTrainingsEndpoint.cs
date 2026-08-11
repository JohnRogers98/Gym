using Ardalis.ApiEndpoints;
using AutoMapper;
using Gym.Abstractions.Query.PersonalTrainings;
using Gym.Application.Services.PersonalTrainingApi.GetPersonalTrainingsByClientId;
using Gym.WebApi.Extensions;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.PersonalTraining;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Gym.WebApi.Controllers.Api.PersonalTrainings.ClientOnly
{
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.Client))]
    public class ClientPersonalTrainingsEndpoint(IMediator _mediator, IMapper _mapper) : EndpointBaseAsync
        .WithoutRequest
        .WithActionResult<ListResponse<PersonalTrainingDto>>
    {
        [HttpGet("api/clients/me/personal-trainings")]
        public async override Task<ActionResult<ListResponse<PersonalTrainingDto>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            IEnumerable<PersonalTrainingProjection> personalTrainings = 
                await _mediator.Send(new GetPersonalTrainingsByClientId(User.GetRequiredUserId()), cancellationToken);

            var response = new ListResponse<PersonalTrainingDto>(_mapper.Map<IEnumerable<PersonalTrainingDto>>(personalTrainings));
            return base.Ok(response);
        }
    }
}
