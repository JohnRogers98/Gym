using Ardalis.ApiEndpoints;
using Gym.Application.Services.PersonalTrainingApi.CreatePersonalTraining;
using Gym.Domain._Shared.Errors;
using Gym.Domain.ClientContext.Errors;
using Gym.Domain.InstructorContext.Errors;
using Gym.WebApi.Extensions;
using Gym.WebDto.Requests.PersonalTraining;
using Gym.WebDto.Responses.PersonalTraining;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Gym.WebApi.Controllers.Api.PersonalTrainings.InstructorOnly
{
    [ApiController]
    [Authorize(Policy = nameof(SecurityPolicy.Instructor))]
    public class CreatePersonalTrainingEndpoint(IMediator _mediator) : EndpointBaseAsync
        .WithRequest<CreatePersonalTrainingRequest>
        .WithActionResult<CreatePersonalTrainingResponse>
    {
        [HttpPost("api/personal-trainings")]
        public override async Task<ActionResult<CreatePersonalTrainingResponse>> HandleAsync(CreatePersonalTrainingRequest request, CancellationToken cancellationToken = default)
        {
            CreatePersonalTraining createPersonalTraining = new(
                User.GetRequiredUserId(),
                request.ClientId,
                request.Start,
                request.End,                
                request.IsPaid,
                request.InstructorComment    
            );

            var createPersonalTrainingResult = await _mediator.Send(createPersonalTraining, cancellationToken);

            if (createPersonalTrainingResult.Success)
            {
                return Accepted(
                  $"api/personal-trainings/{createPersonalTrainingResult.Data!.PersonalTrainingId}",
                  value: new CreatePersonalTrainingResponse() { PersonalTrainingId = createPersonalTrainingResult.Data!.PersonalTrainingId });
            }

            return createPersonalTrainingResult.Error switch
            {
                InstructorIdValidationError 
                or ClientIdValidationError 
                or ClientNotFoundError 
                or StartsAtValidationError
                or EndsAtValidationError
                or TrainingPeriodValidationError => this.BadRequestProblem(createPersonalTrainingResult.Error.GetErrorMessage()),
                _ => this.InternalErrorProblem(createPersonalTrainingResult.Error!.GetErrorMessage())
            };
        }
    }
}
