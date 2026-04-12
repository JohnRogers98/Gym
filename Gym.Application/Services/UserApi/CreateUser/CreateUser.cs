using Gym.Application.Aspects;
using Gym.Domain._Common;
using MediatR;

namespace Gym.Application.Services.UserApi.CreateUser
{
    public record CreateUser(String Login, String Role, String FirstName, String? LastName) : IRequest<Result<CreateUserResult>>, ITransactionalRequest;
}
