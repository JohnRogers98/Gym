using Gym.Domain.TrainingContext;
using MediatR;

namespace Gym.Application.Services.TrainingApi.GetTrainingById
{
    internal class GetTrainingByIdHandler(ITrainingRepository _trainingRepository) : IRequestHandler<GetTrainingById, TrainingDetails>
    {
        public async Task<TrainingDetails> Handle(GetTrainingById request, CancellationToken cancellationToken)
        {
            Training? training = await _trainingRepository.GetByIdAsync(TrainingId.From(request.Id), cancellationToken);

            if (training is null) throw new ArgumentException();

            return training.ToDetails();
        }
    }
}
