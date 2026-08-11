using Ardalis.ApiEndpoints;
using AutoMapper;
using Gym.Application.Services.TrainingApi.CreateTraining;
using Gym.Domain._Common;
using Gym.Domain.TrainingContext.Errors;
using Gym.WebApi.Extensions;
using Gym.WebDto.Requests.Training;
using Gym.WebDto.Responses.Training;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.Trainings.AdminOnly
{
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.Admin))]
    public class CreateTrainingEndpoint(IMediator _mediator, IMapper _mapper) : EndpointBaseAsync
        .WithRequest<CreateTrainingRequest>
        .WithActionResult<CreateTrainingResponse>
    {
        [HttpPost("api/trainings")]
        public override async Task<ActionResult<CreateTrainingResponse>> HandleAsync(CreateTrainingRequest request, CancellationToken cancellationToken = default)
        {
            Result<CreateTrainingResult> createTrainingResult = await _mediator.Send(_mapper.Map<CreateTraining>(request), cancellationToken);

            if (createTrainingResult.Success)
            {
                return Accepted(
                $"api/trainings/{createTrainingResult.Data!.TrainingId}",
                value: _mapper.Map<CreateTrainingResponse>(createTrainingResult.Data));
            }

            return createTrainingResult.Error switch
            {
                TrainingNameValidationError => this.BadRequestProblem(createTrainingResult.Error.GetErrorMessage()),
                _ => this.InternalErrorProblem(createTrainingResult.Error!.GetErrorMessage())
            };
        }

    }
}