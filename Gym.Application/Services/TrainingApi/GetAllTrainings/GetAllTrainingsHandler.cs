using Gym.Domain.TrainingContext;
using MediatR;

namespace Gym.Application.Services.TrainingApi.GetAllTrainings
{
    internal class GetAllTrainingsHandler(ITrainingQueryService _trainingQueryService) : IRequestHandler<GetAllTrainings, IEnumerable<TrainingDetails>>
    {
        public async Task<IEnumerable<TrainingDetails>> Handle(GetAllTrainings request, CancellationToken cancellationToken)
        {
            var trainings = await _trainingQueryService.GetAllAsync(cancellationToken);
            return trainings.Select(aTraining => aTraining.ToDetails());
        }
    }
}
