using Gym.Abstractions.Query.Trainings;
using MediatR;

namespace Gym.Application.Services.TrainingApi.GetAllTrainings
{
    internal class GetAllTrainingsHandler(ITrainingProjectionQueryService _trainingProjectionQueryService) : IRequestHandler<GetAllTrainings, IEnumerable<TrainingProjection>>
    {
        public async Task<IEnumerable<TrainingProjection>> Handle(GetAllTrainings request, CancellationToken cancellationToken)
        {
            return await _trainingProjectionQueryService.GetAllAsync(cancellationToken);
        }
    }
}
