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
    [Route("api/instructors")]
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.AuthenticatedOnly))]
    public class ListInstructorsController(IMediator _mediator, IMapper _mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ListResponse<InstructorDto>>> ListInstructors()
        {
            IEnumerable<InstructorProjection> instructorProjections = await _mediator.Send(new GetAllInstructors());
            
            var response = new ListResponse<InstructorDto>(_mapper.Map<IEnumerable<InstructorDto>>(instructorProjections));
            return base.Ok(response);
        }
    }
}
