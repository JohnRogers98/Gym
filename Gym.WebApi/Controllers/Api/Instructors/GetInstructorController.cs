using AutoMapper;
using Gym.Application.Services.InstructorApi;
using Gym.Application.Services.InstructorApi.GetInstructorById;
using Gym.WebApi.Extensions;
using Gym.WebDto.Responses.Instructor;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.Instructors
{
    [Route("api/instructors/{id}")]
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.AuthenticatedOnly))]
    public class GetInstructorController(IMediator _mediator, IMapper _mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<GetInstructorResponse> GetInstructor(String id)
        {
            InstructorDetails instructorDetails = await _mediator.Send(new GetInstructorById(id));
            return _mapper.Map<GetInstructorResponse>(instructorDetails);
        }
    }
}
