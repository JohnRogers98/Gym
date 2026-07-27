using Gym.Abstractions.Query.PersonalTrainings;
using MediatR;

namespace Gym.Application.Services.PersonalTrainingApi.GetAllPersonalTrainings
{
    public record GetAllPersonalTrainings : IRequest<IEnumerable<PersonalTrainingProjection>>;
}
