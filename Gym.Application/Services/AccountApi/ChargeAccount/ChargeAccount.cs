using MediatR;

namespace Gym.Application.Services.AccountApi.ChargeAccount
{
    public record ChargeAccount(String ClientId, Int32 ByCount) : IRequest<AccountDetails>;
}
