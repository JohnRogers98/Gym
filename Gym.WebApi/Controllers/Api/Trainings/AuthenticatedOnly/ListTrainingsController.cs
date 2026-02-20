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
    [Route("api/trainings")]
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.AuthenticatedOnly))]
    public class ListTrainingsController(IMediator _mediator, IMapper _mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<ListResponse<TrainingDto>> ListTrainings()
        {
            IEnumerable<TrainingProjection> trainingProjections = await _mediator.Send(new GetAllTrainings());
            return new (_mapper.Map<IEnumerable<TrainingDto>>(trainingProjections));
        }
    }
}
