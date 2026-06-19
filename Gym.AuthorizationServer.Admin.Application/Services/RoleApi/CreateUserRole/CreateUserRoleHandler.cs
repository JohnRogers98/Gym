using Gym.AuthorizationServer.Admin.Application.Abstractions;
using Gym.AuthorizationServer.Infrastructure.Entities.Roles;
using MediatR;

namespace Gym.AuthorizationServer.Admin.Application.Services.RoleApi.CreateUserRole
{
    internal class CreateUserRoleHandler(IRoleRepository _roleRepository) : IRequestHandler<CreateUserRole, Result<UserRole>>
    {
        public async Task<Result<UserRole>> Handle(CreateUserRole request, CancellationToken cancellationToken)
        {
            Boolean roleExists = await _roleRepository.ExistsByNameAsync(request.Name, cancellationToken);
            if(roleExists)
                return Result<UserRole>.Failure("role_exists", "Such role already registered");

            UserRoleEntity userRoleEntity = new()
            {
                Name = request.Name
            };
            await _roleRepository.AddAsync(userRoleEntity, cancellationToken);

            return Result<UserRole>.Success(new UserRole { Id = userRoleEntity.Id, Name = userRoleEntity.Name} );
        }
    }
}
