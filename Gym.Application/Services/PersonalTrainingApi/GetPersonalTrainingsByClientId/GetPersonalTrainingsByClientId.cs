using Gym.Abstractions.Query.PersonalTrainings;
using MediatR;

namespace Gym.Application.Services.PersonalTrainingApi.GetPersonalTrainingsByClientId
{
    public record GetPersonalTrainingsByClientId(String ClientId) : IRequest<IEnumerable<PersonalTrainingProjection>>;
}
