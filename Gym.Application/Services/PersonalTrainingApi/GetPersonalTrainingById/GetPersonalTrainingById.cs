using Gym.Abstractions.Query.PersonalTrainings;
using MediatR;

namespace Gym.Application.Services.PersonalTrainingApi.GetPersonalTrainingById
{
    public record GetPersonalTrainingById(String PersonalTrainingId) : IRequest<PersonalTrainingProjection?>;
}
