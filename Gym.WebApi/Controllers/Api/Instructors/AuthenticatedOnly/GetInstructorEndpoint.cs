using Ardalis.ApiEndpoints;
using AutoMapper;
using Gym.Abstractions.Query.Instructors;
using Gym.Application.Services.InstructorApi.GetInstructorById;
using Gym.WebApi.Extensions;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.Instructor;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Gym.WebApi.Controllers.Api.Instructors.AuthenticatedOnly.GetInstructorEndpoint;

namespace Gym.WebApi.Controllers.Api.Instructors.AuthenticatedOnly
{
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.AuthenticatedOnly))]
    public class GetInstructorEndpoint(IMediator _mediator, IMapper _mapper) : EndpointBaseAsync
        .WithRequest<GetInstructorByIdContainer>
        .WithActionResult<Response<InstructorDto>>
    {
        [HttpGet("api/instructors/{id}")]
        public override async Task<ActionResult<Response<InstructorDto>>> HandleAsync(GetInstructorByIdContainer request, CancellationToken cancellationToken = default)
        {
            InstructorProjection? instructorProjection = await _mediator.Send(new GetInstructorById(request.Id), cancellationToken);

            if (instructorProjection is null)
            {
                return base.NotFound($"Instructor with id - {request.Id} not found.");
            }

            var response = new Response<InstructorDto>(_mapper.Map<InstructorDto>(instructorProjection));
            return base.Ok(response);
        }

        public class GetInstructorByIdContainer
        {
            [FromRoute] public String Id { get; set; } = default!;
        }

    }
}
