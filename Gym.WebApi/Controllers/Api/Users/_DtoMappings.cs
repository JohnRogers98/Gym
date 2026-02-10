using AutoMapper;
using Gym.Application.Services.UserApi;
using Gym.WebDto.Responses.Users;

namespace Gym.WebApi.Controllers.Api.Users
{
    public class _DtoMappings : Profile
    {
        public _DtoMappings() 
        {
            CreateMap<UserDetails, WebAppAuthResponse>();
        }
    }
}
