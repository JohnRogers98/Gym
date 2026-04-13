using Gym.Abstractions.Query.EventStore;
using Gym.Application.Extensions;
using Gym.Domain._Shared;
using Gym.Domain.AccountContext.ValueObjects;
using Gym.Domain.ClientContext;
using MediatR;

namespace Gym.Application.Services.AccountApi.GetAccountHistoryByUserId
{
    internal class GetAccountHistoryByUserIdHandler( 
        IClientByUserIdFinder _clientByUserIdFinder,
        IEventProjectionQueryService _eventProjectionQueryService) : IRequestHandler<GetAccountHistoryByUserId, IEnumerable<EventProjection>>
    {
        public async Task<IEnumerable<EventProjection>> Handle(GetAccountHistoryByUserId request, CancellationToken cancellationToken)
        {
            UserId userId = UserId.From(request.UserId).Unwrap();

            Boolean isClient = await _clientByUserIdFinder.ExistsByUserIdAsync(userId, cancellationToken);

            if(isClient is false)
                throw new ArgumentException($"User - {request.UserId} is not client.");

            AccountId accountId = AccountId.From(userId);
            return await _eventProjectionQueryService.GetByStreamId(accountId.Value, cancellationToken);
        }
    }
}
