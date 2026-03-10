using Gym.Domain._Shared.Services;
using Gym.Domain.AccountContext;
using Gym.Domain.ClientContext;
using MediatR;

namespace Gym.Application.Services.AccountApi.ChargeAccount
{
    internal class ChargeAccountHandler(
        IAccountRepository _accountRepository,
        IClientRepository _clientRepository,
        IChargeAccountService _chargeAccountService) : IRequestHandler<ChargeAccount, ChargeAccountResult>
    {
        public async Task<ChargeAccountResult> Handle(ChargeAccount request, CancellationToken cancellationToken)
        {
            ClientId clientId = ClientId.From(request.ClientId);
            Client? client = await _clientRepository.GetByIdAsync(clientId, cancellationToken)
                ?? throw new ArgumentException($"Client id - {clientId} not exist"); ;

            AccountId accountId = AccountId.From(client.UserId);
            Account account = await _accountRepository.GetByIdAsync(accountId, cancellationToken);

            _chargeAccountService.ChargeAccount(account, request.ByCount);

            await _accountRepository.SaveAsync(account, cancellationToken);

            return new ChargeAccountResult(account.AvailableTrainingsCount);
        }
    }
}
