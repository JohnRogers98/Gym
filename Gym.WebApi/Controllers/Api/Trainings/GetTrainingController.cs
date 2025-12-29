using AutoMapper;
using Gym.Application.Services.TrainingApi;
using Gym.Application.Services.TrainingApi.GetTrainingById;
using Gym.WebApi.Extensions;
using Gym.WebDto.Responses.Training;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.Trainings
{
    [Route("api/trainings")]
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.RequireAuthenticated))]
    public class GetTrainingController(IMediator _mediator, IMapper _mapper) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<GetTrainingResponse> GetTraining(String id)
        {
            TrainingDetails trainingDetails = await _mediator.Send(new GetTrainingByIdQuery(id));
            return _mapper.Map<GetTrainingResponse>(trainingDetails);
        }
    }
}
