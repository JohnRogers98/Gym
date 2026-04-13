using Gym.Abstractions.Query.Clients;
using MediatR;

namespace Gym.Application.Services.ClientApi.GetClientByUserId
{
    public record GetClientByUserId(String UserId) : IRequest<ClientProjection?>;
}
