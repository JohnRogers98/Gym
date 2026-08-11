using Gym.Domain._Common;
using MediatR;

namespace Gym.Application.Services.PersonalTrainingApi.CancelPersonalTrainingByInstructorCommand
{
    public record CancelPersonalTrainingByInstructorCommand(String InstructorId, String PersonalTrainingId) : IRequest<Result>;
}
