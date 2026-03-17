using Gym.Application.Aspects;
using Gym.Domain._Common;
using MediatR;

namespace Gym.Application.Services.InstructorApi.CreateInstructor
{
    public record CreateInstructor(String FirstName, String LastName) : IRequest<Result<CreateInstructorResult>>, ITransactionalRequest;
}
