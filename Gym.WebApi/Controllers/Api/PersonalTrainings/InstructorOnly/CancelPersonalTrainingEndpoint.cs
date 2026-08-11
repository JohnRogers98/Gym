using Ardalis.ApiEndpoints;
using Gym.Application.Services.PersonalTrainingApi.CancelPersonalTrainingByInstructorCommand;
using Gym.Domain.InstructorContext.Errors;
using Gym.Domain.PersonalTrainingContext.Errors;
using Gym.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Gym.WebApi.Controllers.Api.PersonalTrainings.InstructorOnly
{
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.Instructor))]
    public class CancelPersonalTrainingEndpoint(IMediator _mediator) : EndpointBaseAsync
        .WithRequest<CancelPersonalTrainingRequest>
        .WithoutResult
    {
        [HttpPost("api/personal-trainings/{id}/cancel")]
        public override async Task<IActionResult> HandleAsync(CancelPersonalTrainingRequest request, CancellationToken cancellationToken = default)
        {
            CancelPersonalTrainingByInstructorCommand cancelPersonalTraining = new(
                User.GetRequiredUserId(),
                request.Id
            );

            var cancelPersonalTrainingResult = await _mediator.Send(cancelPersonalTraining, cancellationToken);

            if (cancelPersonalTrainingResult.Success)
            {
                return base.Ok();
            }

            return cancelPersonalTrainingResult.Error switch
            {
                InstructorIdValidationError
                or InstructorNotFoundError
                or PersonalTrainingIdValidationError
                or PersonalTrainingNotFoundError => this.BadRequestProblem(cancelPersonalTrainingResult.Error.GetErrorMessage()),

                CancelPersonalTrainingError => this.ConflictProblem(cancelPersonalTrainingResult.Error.GetErrorMessage()),

                _ => this.InternalErrorProblem(cancelPersonalTrainingResult.Error!.GetErrorMessage())
            };
        }
    }

    public class CancelPersonalTrainingRequest
    {
        [FromRoute] public String Id { get; set; } = default!;
    }

}
