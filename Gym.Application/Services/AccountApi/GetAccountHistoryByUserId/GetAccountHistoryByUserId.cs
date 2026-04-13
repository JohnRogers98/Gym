using Gym.Abstractions.Query.EventStore;
using MediatR;

namespace Gym.Application.Services.AccountApi.GetAccountHistoryByUserId
{
    public record GetAccountHistoryByUserId(String UserId) : IRequest<IEnumerable<EventProjection>>;
}
