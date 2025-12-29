using AutoMapper;
using Gym.Application.Services.TrainingApi;
using Gym.Application.Services.TrainingApi.CreateTraining;
using Gym.WebApi.Extensions;
using Gym.WebDto.Requests.Training;
using Gym.WebDto.Responses.Training;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.Trainings
{
    [Route("api/trainings")]
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.AdminOnly))]
    public class CreateTrainingController(IMediator _mediator, IMapper _mapper) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<CreateTrainingResponse>> CreateTraining(CreateTrainingRequest request)
        {
            TrainingDetails trainingDetails = await _mediator.Send(_mapper.Map<CreateTrainingCommand>(request));
            
            return base.CreatedAtAction(
                nameof(GetTrainingController.GetTraining),
                "GetTraining",
                new { trainingDetails.id },
                _mapper.Map<CreateTrainingResponse>(trainingDetails));
        }
    }
}