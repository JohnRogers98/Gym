using Gym.Abstractions.Query.EventStore;
using Gym.Application.Extensions;
using Gym.Domain.AccountContext.ValueObjects;
using Gym.Domain.ClientContext;
using Gym.Domain.ClientContext.ValueObjects;
using MediatR;

namespace Gym.Application.Services.AccountApi.GetAccountHistory
{
    internal class GetAccountHistoryHandler( 
        IClientRepository _clientRepository,
        IEventProjectionQueryService _eventProjectionQueryService) : IRequestHandler<GetAccountHistory, IEnumerable<EventProjection>>
    {
        public async Task<IEnumerable<EventProjection>> Handle(GetAccountHistory request, CancellationToken cancellationToken)
        {
            ClientId clientId = ClientId.From(request.ClientId).Unwrap();
            Client client = await _clientRepository.GetByIdAsync(clientId, cancellationToken)
                ?? throw new ArgumentException($"Client id - {clientId} does not exist."); ;

            AccountId accountId = AccountId.From(client.UserId);
            return await _eventProjectionQueryService.GetByStreamId(accountId.Value, cancellationToken);
        }
    }
}
