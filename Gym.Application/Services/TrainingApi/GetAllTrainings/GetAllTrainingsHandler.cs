using Gym.Application.Extensions;
using Gym.Domain.TrainingAggregate;
using MediatR;

namespace Gym.Application.Services.TrainingApi.GetAllTrainings
{
    internal class GetAllTrainingsHandler(ITrainingQueryService _trainingQueryService) : IRequestHandler<GetAllTrainingsQuery, IEnumerable<TrainingDetails>>
    {
        public async Task<IEnumerable<TrainingDetails>> Handle(GetAllTrainingsQuery request, CancellationToken cancellationToken)
        {
            var trainings = await _trainingQueryService.GetAllAsync(cancellationToken);
            return trainings.Select(aTraining => aTraining.ToDetails());
        }
    }
}
