using Gym.Abstractions.Query.Trainings;
using MediatR;

namespace Gym.Application.Services.TrainingApi.GetTrainingById
{
    internal class GetTrainingByIdHandler(ITrainingProjectionQueryService _trainingProjectionQueryService) : IRequestHandler<GetTrainingById, TrainingProjection?>
    {
        public async Task<TrainingProjection?> Handle(GetTrainingById request, CancellationToken cancellationToken)
        {
            return await _trainingProjectionQueryService.GetByIdAsync(request.Id, cancellationToken);
        }
    }
}
