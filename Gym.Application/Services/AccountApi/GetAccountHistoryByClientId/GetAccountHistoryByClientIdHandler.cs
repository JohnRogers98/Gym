using Gym.Abstractions.Query.EventStore;
using Gym.Application.Extensions;
using Gym.Domain.AccountContext.ValueObjects;
using Gym.Domain.ClientContext;
using Gym.Domain.ClientContext.ValueObjects;
using MediatR;

namespace Gym.Application.Services.AccountApi.GetAccountHistoryByUserId
{
    internal class GetAccountHistoryByClientIdHandler(
        IClientRepository _clientRepository,
        IEventProjectionQueryService _eventProjectionQueryService) : IRequestHandler<GetAccountHistoryByClientId, IEnumerable<EventProjection>>
    {
        public async Task<IEnumerable<EventProjection>> Handle(GetAccountHistoryByClientId request, CancellationToken cancellationToken)
        {
            ClientId clientId = ClientId.From(request.ClientId).Unwrap();

            Client client = await _clientRepository.GetByIdAsync(clientId, cancellationToken)
               ?? throw new ArgumentException($"Client id - {clientId} does not exist.");

            AccountId accountId = AccountId.From(client.UserId);
            return await _eventProjectionQueryService.GetByStreamId(accountId.Value, cancellationToken);
        }
    }
}
