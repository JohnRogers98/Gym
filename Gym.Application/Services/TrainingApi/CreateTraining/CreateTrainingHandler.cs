using Gym.Application.Extensions;
using Gym.Domain.TrainingAggregate;
using MediatR;

namespace Gym.Application.Services.TrainingApi.CreateTraining
{
    internal class CreateTrainingHandler(ITrainingRepository _trainingRepository) : IRequestHandler<CreateTrainingCommand, TrainingDetails>
    {
        public async Task<TrainingDetails> Handle(CreateTrainingCommand request, CancellationToken cancellationToken)
        {
            Training training = Training.Create(_trainingRepository.NextIdentity(), request.name, request.description);
            await _trainingRepository.SaveAsync(training, cancellationToken);

            return training.ToDetails();
        }
    }
}
