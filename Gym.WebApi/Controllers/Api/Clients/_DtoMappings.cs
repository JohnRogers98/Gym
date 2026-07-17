using AutoMapper;
using Gym.Abstractions.Query.Clients;
using Gym.Application.Services.UserApi.CreateClient;
using Gym.WebDto.Requests.Client;
using Gym.WebDto.Responses.Clients;

namespace Gym.WebApi.Controllers.Api.Clients
{
    public class _DtoMappings : Profile
    {
        public _DtoMappings()
        {
            CreateMap<ClientProjection, ClientDto>();
        }
    }
}
