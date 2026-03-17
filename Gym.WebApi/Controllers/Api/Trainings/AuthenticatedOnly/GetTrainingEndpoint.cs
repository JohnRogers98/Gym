using Ardalis.ApiEndpoints;
using AutoMapper;
using Gym.Abstractions.Query.Trainings;
using Gym.Application.Services.TrainingApi.GetTrainingById;
using Gym.WebApi.Extensions;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.Training;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Gym.WebApi.Controllers.Api.Trainings.AuthenticatedOnly.GetTrainingEndpoint;

namespace Gym.WebApi.Controllers.Api.Trainings.AuthenticatedOnly
{
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.AuthenticatedOnly))]
    public class GetTrainingEndpoint(IMediator _mediator, IMapper _mapper) : EndpointBaseAsync
        .WithRequest<GetTrainingByIdContainer>
        .WithActionResult<Response<TrainingDto>>
    {
        [HttpGet("api/trainings/{id}")]
        public override async Task<ActionResult<Response<TrainingDto>>> HandleAsync(GetTrainingByIdContainer request, CancellationToken cancellationToken = default)
        {
            TrainingProjection? trainingProjection = await _mediator.Send(new GetTrainingById(request.Id), cancellationToken);

            if(trainingProjection is null)
            {
                return base.NotFound($"Training with id - {request.Id} not found.");
            }

            var response = new Response<TrainingDto>(_mapper.Map<TrainingDto>(trainingProjection));
            return base.Ok(response);
        }

        public class GetTrainingByIdContainer
        {
            [FromRoute] public String Id { get; set; } = default!;
        }

    }
}
