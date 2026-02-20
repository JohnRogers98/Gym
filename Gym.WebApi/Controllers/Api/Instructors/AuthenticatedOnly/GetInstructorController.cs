using AutoMapper;
using Gym.Abstractions.Query.Instructors;
using Gym.Application.Services.InstructorApi.GetInstructorById;
using Gym.WebApi.Extensions;
using Gym.WebDto.Responses.Instructor;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.Instructors.AuthenticatedOnly
{
    [Route("api/instructors/{id}")]
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.AuthenticatedOnly))]
    public class GetInstructorController(IMediator _mediator, IMapper _mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<GetInstructorResponse> GetInstructor(String id)
        {
            InstructorProjection instructorProjection = await _mediator.Send(new GetInstructorById(id));
            return _mapper.Map<GetInstructorResponse>(instructorProjection);
        }
    }
}
