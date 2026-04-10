using AutoMapper;
using Gym.Application.Services.UserApi;
using Gym.Application.Services.UserApi.ChangePassword;
using Gym.Application.Services.UserApi.CreateUser;
using Gym.WebDto.Requests.Users;
using Gym.WebDto.Responses.Users;

namespace Gym.WebApi.Controllers.Api.Users
{
    public class _DtoMappings : Profile
    {
        public _DtoMappings() 
        {
            CreateMap<AuthenticatedUserDetails, AuthResponse>();

            CreateMap<CreateUserRequest, CreateUser>();
            CreateMap<CreateUserResult, CreateUserResponse>();
        }
    }
}
