using AutoMapper;
using Gym.Application.Services.InstructorApi;
using Gym.Application.Services.InstructorApi.GetAllInstructors;
using Gym.WebApi.Extensions;
using Gym.WebDto.Dto;
using Gym.WebDto.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.Instructors
{
    [Route("api/instructors")]
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.RequireAuthenticated))]
    public class ListInstructorsController(IMediator _mediator, IMapper _mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<ListResponse<InstructorDto>> ListInstructors()
        {
            IEnumerable<InstructorDetails> instructorDetails = await _mediator.Send(new GetAllInstructorsQuery());
            return new (_mapper.Map<IEnumerable<InstructorDto>>(instructorDetails));
        }
    }
}
