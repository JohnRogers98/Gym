using Gym.Abstractions.Query.Clients;
using MediatR;

namespace Gym.Application.Services.ClientApi.GetClientByUserId
{
    internal class GetClientByUserIdHandler(IClientProjectionQueryService _clientProjectionQueryService) : IRequestHandler<GetClientByUserId, ClientProjection?>
    {
        public async Task<ClientProjection?> Handle(GetClientByUserId request, CancellationToken cancellationToken)
        {
            return await _clientProjectionQueryService.GetByUserIdAsync(request.UserId, cancellationToken);
        }
    }
}
