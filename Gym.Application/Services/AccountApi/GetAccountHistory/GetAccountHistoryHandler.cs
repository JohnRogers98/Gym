using Gym.Abstractions.Query.EventStore;
using Gym.Domain.AccountContext;
using Gym.Domain.ClientContext;
using MediatR;

namespace Gym.Application.Services.AccountApi.GetAccountHistory
{
    internal class GetAccountHistoryHandler( 
        IClientRepository _clientRepository,
        IEventProjectionQueryService _eventProjectionQueryService) : IRequestHandler<GetAccountHistory, IEnumerable<EventProjection>>

    {
        public async Task<IEnumerable<EventProjection>> Handle(GetAccountHistory request, CancellationToken cancellationToken)
        {
            ClientId clientId = ClientId.From(request.ClientId);
            Client? client = await _clientRepository.GetByIdAsync(clientId, cancellationToken)
                ?? throw new ArgumentException($"Client id - {clientId} not exist"); ;

            AccountId accountId = AccountId.From(client.UserId);
            return await _eventProjectionQueryService.GetByStreamId(accountId.Value, cancellationToken);
        }
    }
}
