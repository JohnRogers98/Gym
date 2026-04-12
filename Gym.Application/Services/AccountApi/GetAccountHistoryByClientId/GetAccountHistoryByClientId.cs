using Gym.Abstractions.Query.EventStore;
using MediatR;

namespace Gym.Application.Services.AccountApi.GetAccountHistoryByUserId
{
    public record GetAccountHistoryByClientId(String ClientId) : IRequest<IEnumerable<EventProjection>>;
}
