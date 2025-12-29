using AutoMapper;
using Gym.Application.Services.InstructorApi;
using Gym.Application.Services.InstructorApi.CreateInstructor;
using Gym.WebApi.Extensions;
using Gym.WebDto.Requests.Instructor;
using Gym.WebDto.Responses.Instructor;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.Instructors
{
    [Route("api/instructors")]
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.AdminOnly))]
    public class CreateTrainingController(IMediator _mediator, IMapper _mapper) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<CreateInstructorResponse>> CreateInstructor(CreateInstructorRequest request)
        {
            InstructorDetails instructorDetails = await _mediator.Send(_mapper.Map<CreateInstructorCommand>(request));

            return base.CreatedAtAction(
                nameof(GetInstructorController.GetInstructor),
                "GetInstructor",
                new { instructorDetails.id },
                _mapper.Map<CreateInstructorResponse>(instructorDetails));
        }
    }
}