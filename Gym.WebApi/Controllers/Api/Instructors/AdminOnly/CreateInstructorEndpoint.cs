using Ardalis.ApiEndpoints;
using AutoMapper;
using Gym.Application.Services.InstructorApi.CreateInstructor;
using Gym.Domain._Common;
using Gym.Domain._Shared.Errors;
using Gym.WebApi.Extensions;
using Gym.WebDto.Requests.Instructor;
using Gym.WebDto.Responses.Instructor;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers.Api.Instructors.AdminOnly
{
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.Admin))]
    public class CreateInstructorEndpoint(IMediator _mediator, IMapper _mapper) : EndpointBaseAsync
        .WithRequest<CreateInstructorRequest>
        .WithActionResult<CreateInstructorResponse>
    {
        [HttpPost("api/instructors")]
        public override async Task<ActionResult<CreateInstructorResponse>> HandleAsync(CreateInstructorRequest request, CancellationToken cancellationToken = default)
        {
            Result<CreateInstructorResult> createInstructorResult = await _mediator.Send(_mapper.Map<CreateInstructor>(request), cancellationToken);

            if (createInstructorResult.Success)
            {
                return Accepted(
                  $"api/instructors/{createInstructorResult.Data!.InstructorId}",
                  value: _mapper.Map<CreateInstructorResponse>(createInstructorResult.Data));
            }

            return createInstructorResult.Error switch
            {
                FirstNameValidationError or LastNameValidationError => this.BadRequestProblem(createInstructorResult.Error.GetErrorMessage()),
                _ => this.InternalErrorProblem(createInstructorResult.Error!.GetErrorMessage())
            };
        }

    }
}