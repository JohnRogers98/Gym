using Ardalis.ApiEndpoints;
using AutoMapper;
using Gym.Abstractions.Query.PersonalTrainings;
using Gym.Application.Services.PersonalTrainingApi.GetPersonalTrainingsByInstructorId;
using Gym.WebApi.Extensions;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.PersonalTraining;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.PersonalTrainings.InstructorOnly
{
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.InstructorOnly))]
    public class InstructorPersonalTrainingsEndpoint(IMediator _mediator, IMapper _mapper) : EndpointBaseAsync
        .WithoutRequest
        .WithActionResult<ListResponse<PersonalTrainingDto>>
    {
        [HttpGet("api/instructors/me/personal-trainings")]
        public async override Task<ActionResult<ListResponse<PersonalTrainingDto>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            IEnumerable<PersonalTrainingProjection> personalTrainings = await _mediator.Send(new GetPersonalTrainingsByInstructorId(User.GetRequiredUserId()));

            var response = new ListResponse<PersonalTrainingDto>(_mapper.Map<IEnumerable<PersonalTrainingDto>>(personalTrainings));
            return base.Ok(response);
        }
    }
}
