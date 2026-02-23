using Gym.Domain.TrainingContext;
using MediatR;

namespace Gym.Application.Services.TrainingApi.CreateTraining
{
    internal class CreateTrainingHandler(ITrainingRepository _trainingRepository) : IRequestHandler<CreateTraining, CreateTrainingResult>
    {
        public async Task<CreateTrainingResult> Handle(CreateTraining request, CancellationToken cancellationToken)
        {
            Training training = Training.Create(_trainingRepository.NextIdentity(), request.Name, request.Description);
            await _trainingRepository.SaveAsync(training, cancellationToken);

            return new CreateTrainingResult(training.Id.Value);
        }
    }
}
