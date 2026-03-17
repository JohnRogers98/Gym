using Gym.Application.Aspects;
using Gym.Domain._Common;
using MediatR;

namespace Gym.Application.Services.AccountApi.ChargeAccount
{
    public record ChargeAccount(String ClientId, Int32 ByCount) : IRequest<Result<ChargeAccountResult>>, ITransactionalRequest;
}
