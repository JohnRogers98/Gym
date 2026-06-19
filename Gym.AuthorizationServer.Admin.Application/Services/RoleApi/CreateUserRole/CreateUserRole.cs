using Gym.AuthorizationServer.Admin.Application.Abstractions;
using MediatR;

namespace Gym.AuthorizationServer.Admin.Application.Services.RoleApi.CreateUserRole
{
    public record CreateUserRole(String Name) : IRequest<Result<UserRole>>;
}
