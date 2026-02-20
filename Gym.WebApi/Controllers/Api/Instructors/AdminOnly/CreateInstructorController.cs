using AutoMapper;
using Gym.Application.Services.InstructorApi.CreateInstructor;
using Gym.WebApi.Controllers.Api.Instructors.AuthenticatedOnly;
using Gym.WebApi.Extensions;
using Gym.WebDto.Requests.Instructor;
using Gym.WebDto.Responses.Instructor;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.Instructors.AdminOnly
{
    [Route("api/instructors")]
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.AdminOnly))]
    public class CreateTrainingController(IMediator _mediator, IMapper _mapper) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<CreateInstructorResponse>> CreateInstructor(CreateInstructorRequest request)
        {
            CreateInstructorResult createInstructorResult = await _mediator.Send(_mapper.Map<CreateInstructor>(request));

            return base.AcceptedAtAction(
                nameof(GetInstructorController.GetInstructor),
                "GetInstructor",
                new { id = createInstructorResult.InstructorId },
                _mapper.Map<CreateInstructorResponse>(createInstructorResult));
        }
    }
}