using Gym.AuthorizationServer.Infrastructure.Entities.Roles;
using MediatR;

namespace Gym.AuthorizationServer.Admin.Application.Services.RoleApi.GetAllRoles
{
    internal class GetAllUserRolesHandler(IRoleRepository _roleRepository) : IRequestHandler<GetAllUserRoles, IEnumerable<UserRole>>
    {
        public async Task<IEnumerable<UserRole>> Handle(GetAllUserRoles request, CancellationToken cancellationToken)
        {
            var roleEntities = await _roleRepository.GetAllAsync(cancellationToken);
            return roleEntities.Select(entity => new UserRole{ Id = entity.Id, Name = entity.Name});
        }
    }
}
