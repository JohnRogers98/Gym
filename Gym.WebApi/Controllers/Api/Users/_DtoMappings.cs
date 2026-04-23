using AutoMapper;
using Gym.Application.Services.ClientApi.CreateClient;
using Gym.Application.Services.UserApi;
using Gym.Application.Services.UserApi.ChangePassword;
using Gym.WebDto.Requests.Client;
using Gym.WebDto.Responses.Clients;
using Gym.WebDto.Responses.Users;

namespace Gym.WebApi.Controllers.Api.Users
{
    public class _DtoMappings : Profile
    {
        public _DtoMappings() 
        {
            CreateMap<AuthenticatedUserDetails, AuthResponse>();

            CreateMap<CreateClientRequest, CreateClient>();
            CreateMap<CreateClientResult, CreateClientResponse>();
        }
    }
}
