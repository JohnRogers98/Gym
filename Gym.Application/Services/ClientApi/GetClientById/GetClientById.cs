using Gym.Abstractions.Query.Clients;
using MediatR;

namespace Gym.Application.Services.ClientApi.GetClientById
{
    public record GetClientById(String ClientId) : IRequest<ClientProjection?>;
}
