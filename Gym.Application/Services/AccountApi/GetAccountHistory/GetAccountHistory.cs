using MediatR;

namespace Gym.Application.Services.AccountApi.GetAccountHistory
{
    public record GetAccountHistory(String ClientId) : IRequest<IEnumerable<AccountEventDetails>>;
}
