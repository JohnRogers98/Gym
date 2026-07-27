using Gym.AuthorizationServer.Admin.Application.Services.RoleApi;
using Gym.AuthorizationServer.Admin.Application.Services.RoleApi.CreateUserRole;
using Gym.AuthorizationServer.Admin.Application.Services.UserApi;
using Gym.AuthorizationServer.Admin.Application.Services.UserApi.CreateUser;
using Gym.WebDto.Requests.Roles;
using Gym.WebDto.Requests.Users;
using Gym.WebDto.Responses.Roles;
using Gym.WebDto.Responses.Users;

namespace Gym.AuthorizationServer.Admin.Extensions.Mappings
{
    public static class MappingExtensions
    {
        public static CreateUser ToApplicationRequest(this CreateUserRequest request)
        {
            return new(request.Username, request.Password, request.FirstName, request.LastName, request.RoleId);
        }

        public static CreateUserRole ToApplicationRequest(this CreateUserRoleRequest request)
        {
            return new(request.Name);
        }

        public static UserRoleDto ToResponseDto(this UserRole userRole)
        {
            return new() 
            { 
                Id = userRole.Id,
                Name = userRole.Name
            };
        }

        public static UserDto ToResponseDto(this User user)
        {
            return new()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                RoleId = user.RoleId
            };
        }
    }
}
