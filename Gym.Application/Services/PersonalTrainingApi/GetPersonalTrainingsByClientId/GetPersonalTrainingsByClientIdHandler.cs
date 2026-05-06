using Gym.Abstractions.Query.PersonalTrainings;
using MediatR;

namespace Gym.Application.Services.PersonalTrainingApi.GetPersonalTrainingsByClientId
{
    internal class GetPersonalTrainingsByClientIdHandler(IPersonalTrainingProjectionQueryService _personalTrainingProjectionQueryService) 
        : IRequestHandler<GetPersonalTrainingsByClientId, IEnumerable<PersonalTrainingProjection>>
    {
        public async Task<IEnumerable<PersonalTrainingProjection>> Handle(GetPersonalTrainingsByClientId request, CancellationToken cancellationToken)
        {
            return await _personalTrainingProjectionQueryService.GetAllByClientIdAsync(request.ClientId, cancellationToken);
        }
    }
}
