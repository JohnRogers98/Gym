using MediatR;

namespace Gym.AuthorizationServer.Admin.Application.Services.RoleApi.GetAllRoles
{
    public record GetAllUserRoles : IRequest<IEnumerable<UserRole>>;
}
