using Gym.AuthorizationServer.Admin.Application.Abstractions;
using MediatR;

namespace Gym.AuthorizationServer.Admin.Application.Services.UserApi.CreateUser
{
    public record CreateUser(
        String Username,
        String Password,
        String FirstName,
        String? LastName,
        String RoleId) : IRequest<Result<CreateUserResult>>;
}
