using Gym.Domain.TrainingContext;
using MediatR;

namespace Gym.Application.Services.TrainingApi.CreateTraining
{
    internal class CreateTrainingHandler(ITrainingRepository _trainingRepository) : IRequestHandler<CreateTraining, TrainingDetails>
    {
        public async Task<TrainingDetails> Handle(CreateTraining request, CancellationToken cancellationToken)
        {
            Training training = Training.Create(_trainingRepository.NextIdentity(), request.Name, request.Description);
            await _trainingRepository.SaveAsync(training, cancellationToken);

            return training.ToDetails();
        }
    }
}
