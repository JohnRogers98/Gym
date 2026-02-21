using AutoMapper;
using Gym.Application.Services.UserApi.TelegramAuthentication;
using Gym.WebDto.Responses.Users;

namespace Gym.WebApi.Controllers.Api.Users
{
    public class _DtoMappings : Profile
    {
        public _DtoMappings() 
        {
            CreateMap<AuthenticatedUserDetails, WebAppAuthResponse>();
        }
    }
}
