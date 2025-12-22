using AutoMapper;
using Gym.Application.Services.InstructorApi;
using Gym.Application.Services.InstructorApi.GetInstructorById;
using Gym.WebDto.Responses.Instructor;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.Instructors
{
    [Route("api/instructors")]
    [ApiController]
    public class GetInstructorController(IMediator _mediator, IMapper _mapper) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<GetInstructorResponse> GetInstructor(String id)
        {
            InstructorDetails instructorDetails = await _mediator.Send(new GetInstructorByIdQuery(id));
            return _mapper.Map<GetInstructorResponse>(instructorDetails);
        }
    }
}
