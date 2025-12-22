using Gym.Application.Extensions;
using Gym.Domain.TrainingAggregate;
using MediatR;

namespace Gym.Application.Services.TrainingApi.GetTrainingById
{
    internal class GetTrainingByIdHandler(ITrainingRepository _trainingRepository) : IRequestHandler<GetTrainingByIdQuery, TrainingDetails>
    {
        public async Task<TrainingDetails> Handle(GetTrainingByIdQuery request, CancellationToken cancellationToken)
        {
            Training? training = await _trainingRepository.GetByIdAsync(TrainingId.From(request.id), cancellationToken);

            if (training is null) throw new ArgumentException();

            return training.ToDetails();
        }
    }
}
