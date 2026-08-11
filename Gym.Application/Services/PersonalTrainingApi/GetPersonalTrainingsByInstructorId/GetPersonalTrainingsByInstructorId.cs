using Gym.Abstractions.Query.PersonalTrainings;
using MediatR;

namespace Gym.Application.Services.PersonalTrainingApi.GetPersonalTrainingsByInstructorId
{
    public record GetPersonalTrainingsByInstructorId(String InstructorId) : IRequest<IEnumerable<PersonalTrainingProjection>>;
}
