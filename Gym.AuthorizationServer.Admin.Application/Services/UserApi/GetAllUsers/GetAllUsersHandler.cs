using Gym.AuthorizationServer.Infrastructure.Entities.Users;
using MediatR;

namespace Gym.AuthorizationServer.Admin.Application.Services.UserApi.GetAllUsers
{
    internal class GetAllUsersHandler(IUserRepository _userRepository) : IRequestHandler<GetAllUsers, IEnumerable<User>>
    {
        public async Task<IEnumerable<User>> Handle(GetAllUsers request, CancellationToken cancellationToken)
        {
            var userEntities = await _userRepository.GetAllAsync(cancellationToken);
            return userEntities.Select(entity => new User 
            { 
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                RoleId = entity.RoleId
            });
        }
    }
}
