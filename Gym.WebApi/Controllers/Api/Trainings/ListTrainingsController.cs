using AutoMapper;
using Gym.Application.Services.TrainingApi;
using Gym.Application.Services.TrainingApi.GetAllTrainings;
using Gym.WebApi.Extensions;
using Gym.WebDto.Dto;
using Gym.WebDto.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.Trainings
{
    [Route("api/trainings")]
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.RequireAuthenticated))]
    public class ListTrainingsController(IMediator _mediator, IMapper _mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<ListResponse<TrainingDto>> ListTrainings()
        {
            IEnumerable<TrainingDetails> trainingDetails = await _mediator.Send(new GetAllTrainingsQuery());
            return new (_mapper.Map<IEnumerable<TrainingDto>>(trainingDetails));
        }
    }
}
