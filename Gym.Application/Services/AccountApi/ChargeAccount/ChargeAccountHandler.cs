using Gym.Domain._Common;
using Gym.Domain._Shared.Services;
using Gym.Domain.AccountContext;
using Gym.Domain.AccountContext.ValueObjects;
using Gym.Domain.ClientContext;
using Gym.Domain.ClientContext.Errors;
using Gym.Domain.ClientContext.ValueObjects;
using MediatR;

namespace Gym.Application.Services.AccountApi.ChargeAccount
{
    internal class ChargeAccountHandler(
        IAccountRepository _accountRepository,
        IClientRepository _clientRepository,
        IChargeAccountService _chargeAccountService) : IRequestHandler<ChargeAccount, Result<ChargeAccountResult>>
    {
        public async Task<Result<ChargeAccountResult>> Handle(ChargeAccount request, CancellationToken cancellationToken)
        {
            var clientIdResult = ClientId.From(request.ClientId);
            if (clientIdResult.Success is false)
                return Result<ChargeAccountResult>.Fail(clientIdResult.Error!);

            Client? client = await _clientRepository.GetByIdAsync(clientIdResult.Data!, cancellationToken);
            if (client is null)
                return Result<ChargeAccountResult>.Fail(ClientNotFoundError.Create(clientIdResult.Data!));

            AccountId accountId = AccountId.From(client!.UserId);
            Account account = await _accountRepository.GetByIdAsync(accountId, cancellationToken);

            var chargeResult = _chargeAccountService.ChargeAccount(account, request.ByCount);
            if(chargeResult.Success is false)
                return Result<ChargeAccountResult>.Fail(chargeResult.Error!);

            await _accountRepository.SaveAsync(account, cancellationToken);

            return Result<ChargeAccountResult>.Ok(new ChargeAccountResult(account.RemainingTrainings.Value));
        }
    }
}
