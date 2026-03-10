using Gym.Abstractions.Query.Clients;
using MediatR;

namespace Gym.Application.Services.ClientApi.GetAllClients
{
    public record GetAllClients : IRequest<IEnumerable<ClientProjection>>;
}
