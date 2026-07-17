using Ardalis.ApiEndpoints;
using AutoMapper;
using Gym.Abstractions.Query.PersonalTrainings;
using Gym.Application.Services.PersonalTrainingApi.GetPersonalTrainingById;
using Gym.WebApi.Extensions;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.PersonalTraining;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.PersonalTrainings.InstructorOnly
{
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.Instructor))]
    public class GetPersonalTrainingEndpoint(IMediator _mediator, IMapper _mapper) : EndpointBaseAsync
        .WithRequest<GetPersonalTrainingByIdContainer>
        .WithActionResult<Response<PersonalTrainingDto>>
    {
        [HttpGet("api/personal-trainings/{id}")]
        public override async Task<ActionResult<Response<PersonalTrainingDto>>> HandleAsync(GetPersonalTrainingByIdContainer request, CancellationToken cancellationToken = default)
        {
            PersonalTrainingProjection? personalTrainingProjection = await _mediator.Send(new GetPersonalTrainingById(request.Id), cancellationToken);

            if (personalTrainingProjection is null)
            {
                return base.NotFound($"Personal training with id - {request.Id} not found.");
            }

            var response = new Response<PersonalTrainingDto>(_mapper.Map<PersonalTrainingDto>(personalTrainingProjection));
            return base.Ok(response);
        }
    }

    public class GetPersonalTrainingByIdContainer
    {
        [FromRoute] public String Id { get; set; } = default!;
    }
}
