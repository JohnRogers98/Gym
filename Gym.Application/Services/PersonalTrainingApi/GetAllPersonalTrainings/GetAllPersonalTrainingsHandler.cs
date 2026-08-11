using Gym.Abstractions.Query.PersonalTrainings;
using MediatR;

namespace Gym.Application.Services.PersonalTrainingApi.GetAllPersonalTrainings
{
    internal class GetAllPersonalTrainingsHandler(IPersonalTrainingProjectionQueryService _personalTrainingProjectionQueryService)
        : IRequestHandler<GetAllPersonalTrainings, IEnumerable<PersonalTrainingProjection>>
    {
        public async Task<IEnumerable<PersonalTrainingProjection>> Handle(GetAllPersonalTrainings request, CancellationToken cancellationToken)
        {
            return await _personalTrainingProjectionQueryService.GetAllAsync(cancellationToken);
        }
    }
}
