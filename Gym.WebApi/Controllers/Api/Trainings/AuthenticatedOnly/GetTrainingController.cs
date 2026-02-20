using AutoMapper;
using Gym.Abstractions.Query.Trainings;
using Gym.Application.Services.TrainingApi.GetTrainingById;
using Gym.WebApi.Extensions;
using Gym.WebDto.Responses.Training;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.Trainings.AuthenticatedOnly
{
    [Route("api/trainings/{id}")]
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.AuthenticatedOnly))]
    public class GetTrainingController(IMediator _mediator, IMapper _mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<GetTrainingResponse> GetTraining(String id)
        {
            TrainingProjection trainingProjection = await _mediator.Send(new GetTrainingById(id));
            return _mapper.Map<GetTrainingResponse>(trainingProjection);
        }
    }
}
