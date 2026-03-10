using Gym.Abstractions.Query.Clients;
using MediatR;

namespace Gym.Application.Services.ClientApi.GetAllClients
{
    internal class GetAllClientsHandler(IClientProjectionQueryService _clientProjectionQueryService) : IRequestHandler<GetAllClients, IEnumerable<ClientProjection>>
    {
        public async Task<IEnumerable<ClientProjection>> Handle(GetAllClients request, CancellationToken cancellationToken)
        {
            return await _clientProjectionQueryService.GetAllAsync(cancellationToken);
        }
    }
}
