using Gym.Application.Extensions;
using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.TrainingContext;
using Gym.Domain.TrainingContext.ValueObjects;
using MediatR;

namespace Gym.Application.Services.TrainingApi.CreateTraining
{
    internal class CreateTrainingHandler(ITrainingRepository _trainingRepository) : IRequestHandler<CreateTraining, Result<CreateTrainingResult>>
    {
        public async Task<Result<CreateTrainingResult>> Handle(CreateTraining request, CancellationToken cancellationToken)
        {
            var shapeTrainingResult = from trainingName in TrainingName.From(request.Name)
                                      from description in Description.From(request.Description)
                                      select Training.Create(_trainingRepository.NextIdentity(), trainingName, description);

            if (shapeTrainingResult.Success is false)
                return Result<CreateTrainingResult>.Fail(shapeTrainingResult.Error!);

            await _trainingRepository.SaveAsync(shapeTrainingResult.Data!, cancellationToken);

            return Result<CreateTrainingResult>.Ok(new CreateTrainingResult(shapeTrainingResult.Data!.Id.Value));
        }
    }
}
