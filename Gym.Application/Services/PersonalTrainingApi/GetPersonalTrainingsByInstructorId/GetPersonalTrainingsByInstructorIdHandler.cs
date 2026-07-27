using Gym.Abstractions.Query.PersonalTrainings;
using MediatR;

namespace Gym.Application.Services.PersonalTrainingApi.GetPersonalTrainingsByInstructorId
{
    internal class GetPersonalTrainingsByInstructorIdHandler(
        IPersonalTrainingProjectionQueryService _personalTrainingProjectionQueryService) 
        : IRequestHandler<GetPersonalTrainingsByInstructorId, IEnumerable<PersonalTrainingProjection>>
    {
        public async Task<IEnumerable<PersonalTrainingProjection>> Handle(GetPersonalTrainingsByInstructorId request, CancellationToken cancellationToken)
        {
            return await _personalTrainingProjectionQueryService.GetAllByInstructorIdAsync(request.InstructorId, cancellationToken);
        }
    }
}
