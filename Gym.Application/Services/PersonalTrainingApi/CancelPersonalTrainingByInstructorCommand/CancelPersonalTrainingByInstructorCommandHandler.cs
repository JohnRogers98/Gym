using Gym.Domain._Common;
using Gym.Domain.InstructorContext;
using Gym.Domain.InstructorContext.Errors;
using Gym.Domain.InstructorContext.ValueObjects;
using Gym.Domain.PersonalTrainingContext;
using Gym.Domain.PersonalTrainingContext.Errors;
using Gym.Domain.PersonalTrainingContext.ValueObjects;
using MediatR;

namespace Gym.Application.Services.PersonalTrainingApi.CancelPersonalTrainingByInstructorCommand
{
    internal class CancelPersonalTrainingByInstructorCommandHandler(
        IInstructorRepository _instructorRepository,
        IPersonalTrainingRepository _personalTrainingRepository) : IRequestHandler<CancelPersonalTrainingByInstructorCommand, Result>
    {
        public async Task<Result> Handle(CancelPersonalTrainingByInstructorCommand request, CancellationToken cancellationToken)
        {
            var instructorIdResult = InstructorId.From(request.InstructorId);
            if (instructorIdResult.Success is false)
                return Result.Fail(instructorIdResult.Error!);

            var instructorExists = await _instructorRepository.ExistsAsync(instructorIdResult.Data!, cancellationToken);
            if(instructorExists is false)
                return Result.Fail(InstructorNotFoundError.Create(instructorIdResult.Data!));

            var personalTrainingIdResult = PersonalTrainingId.From(request.PersonalTrainingId);
            if(personalTrainingIdResult.Success is false)
                return Result.Fail(personalTrainingIdResult.Error!);

            PersonalTraining? personalTraining = await _personalTrainingRepository.GetByIdAsync(personalTrainingIdResult.Data!, cancellationToken);
            if (personalTraining is null)
                return Result.Fail(PersonalTrainingNotFoundError.Create(personalTrainingIdResult.Data!));

            var cancelResult = personalTraining.Cancel();
            if(cancelResult.Success is false)
                return Result.Fail(cancelResult.Error!);

            await _personalTrainingRepository.SaveAsync(personalTraining, cancellationToken);

            return Result.Ok();
        }
    }
}
