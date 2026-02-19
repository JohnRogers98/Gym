using Gym.Abstractions.Query.EventStore;
using Gym.Domain.AccountContext;
using Gym.Domain.ClientContext;
using MediatR;

namespace Gym.Application.Services.AccountApi.GetAccountHistory
{
    internal class GetAccountHistoryHandler( 
        IClientQueryService _clientQueryService,
        IEventProjectionQueryService _eventProjectionQueryService) : IRequestHandler<GetAccountHistory, IEnumerable<AccountEventDetails>>

    {
        public async Task<IEnumerable<AccountEventDetails>> Handle(GetAccountHistory request, CancellationToken cancellationToken)
        {
            ClientId clientId = ClientId.From(request.ClientId);
            Client? client = await _clientQueryService.GetByIdAsync(clientId, cancellationToken);
            if (client is null)
            {
                throw new ArgumentException($"Client id - {clientId} not exist");
            }

            AccountId accountId = AccountId.From(client.UserId);
            IEnumerable<EventProjection> eventProjections = await _eventProjectionQueryService.GetByStreamId(accountId.Value, cancellationToken);

            return eventProjections.ToDetails();
        }
    }
}
