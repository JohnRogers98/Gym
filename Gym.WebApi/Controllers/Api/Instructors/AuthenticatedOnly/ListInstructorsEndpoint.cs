using Ardalis.ApiEndpoints;
using AutoMapper;
using Gym.Abstractions.Query.Instructors;
using Gym.Application.Services.InstructorApi.GetAllInstructors;
using Gym.WebApi.Extensions;
using Gym.WebDto.Responses;
using Gym.WebDto.Responses.Instructor;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.Instructors.AuthenticatedOnly
{
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.AuthenticatedOnly))]
    public class ListInstructorsEndpoint(IMediator _mediator, IMapper _mapper) : EndpointBaseAsync
        .WithoutRequest
        .WithActionResult<ListResponse<InstructorDto>>
    {
        [HttpGet("api/instructors")]
        public override async Task<ActionResult<ListResponse<InstructorDto>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            IEnumerable<InstructorProjection> instructorProjections = await _mediator.Send(new GetAllInstructors(), cancellationToken);

            var response = new ListResponse<InstructorDto>(_mapper.Map<IEnumerable<InstructorDto>>(instructorProjections));
            return base.Ok(response);
        }
    }
}
