using Gym.Abstractions.Query.Clients;
using MediatR;

namespace Gym.Application.Services.ClientApi.GetClientById
{
    internal class GetClientByIdHandler(IClientProjectionQueryService _clientProjectionQueryService) : IRequestHandler<GetClientById, ClientProjection>
    {
        public async Task<ClientProjection> Handle(GetClientById request, CancellationToken cancellationToken)
        {
            return await _clientProjectionQueryService.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new ArgumentException();
        }
    }
}
