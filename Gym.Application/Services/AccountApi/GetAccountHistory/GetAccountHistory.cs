using Gym.Abstractions.Query.EventStore;
using MediatR;

namespace Gym.Application.Services.AccountApi.GetAccountHistory
{
    public record GetAccountHistory(String ClientId) : IRequest<IEnumerable<EventProjection>>;
}
