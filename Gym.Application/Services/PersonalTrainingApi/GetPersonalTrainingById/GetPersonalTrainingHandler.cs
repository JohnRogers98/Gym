using Gym.Abstractions.Query.PersonalTrainings;
using MediatR;

namespace Gym.Application.Services.PersonalTrainingApi.GetPersonalTrainingById
{
    internal class GetPersonalTrainingHandler(IPersonalTrainingProjectionQueryService _personalTrainingProjectionQueryService) 
        : IRequestHandler<GetPersonalTrainingById, PersonalTrainingProjection?>
    {
        public async Task<PersonalTrainingProjection?> Handle(GetPersonalTrainingById request, CancellationToken cancellationToken)
        {
            return await _personalTrainingProjectionQueryService.GetByIdAsync(request.PersonalTrainingId, cancellationToken);
        }
    }
}
