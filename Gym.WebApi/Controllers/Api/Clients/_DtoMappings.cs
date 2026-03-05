using AutoMapper;
using Gym.Abstractions.Query.Clients;
using Gym.WebDto.Responses.Clients;

namespace Gym.WebApi.Controllers.Api.Clients
{
    public class _DtoMappings : Profile
    {
        public _DtoMappings()
        {
            CreateMap<ClientProjection, ClientDto>();
            CreateMap<ClientProjection, GetClientResponse>();
        }
    }
}
