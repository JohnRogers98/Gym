using Ardalis.ApiEndpoints;
using AutoMapper;
using Gym.Abstractions.Query.Trainings;
using Gym.Application.Services.TrainingApi.GetAllTrainings;
using Gym.WebApi.Extensions;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.Training;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.Trainings.AuthenticatedOnly
{
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.AuthenticatedOnly))]
    public class ListTrainingsEndpoint(IMediator _mediator, IMapper _mapper) : EndpointBaseAsync
        .WithoutRequest
        .WithActionResult<ListResponse<TrainingDto>>
    {
        [HttpGet("api/trainings")]
        public override async Task<ActionResult<ListResponse<TrainingDto>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            IEnumerable<TrainingProjection> trainingProjections = await _mediator.Send(new GetAllTrainings(), cancellationToken);

            var response = new ListResponse<TrainingDto>(_mapper.Map<IEnumerable<TrainingDto>>(trainingProjections));
            return base.Ok(response);
        }
    }
}
