using AutoMapper;
using Gym.Application.Services.TrainingApi.CreateTraining;
using Gym.WebApi.Controllers.Api.Trainings.AuthenticatedOnly;
using Gym.WebApi.Extensions;
using Gym.WebDto.Requests.Training;
using Gym.WebDto.Responses.Training;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.Trainings.AdminOnly
{
    [Route("api/trainings")]
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.AdminOnly))]
    public class CreateTrainingController(IMediator _mediator, IMapper _mapper) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<CreateTrainingResponse>> CreateTraining(CreateTrainingRequest request)
        {
            CreateTrainingResult createTrainingResult = await _mediator.Send(_mapper.Map<CreateTraining>(request));
            
            return base.AcceptedAtAction(
                nameof(GetTrainingController.GetTraining),
                "GetTraining",
                new { id = createTrainingResult.TrainingId },
                _mapper.Map<CreateTrainingResponse>(createTrainingResult));
        }
    }
}