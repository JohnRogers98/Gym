using Gym.Domain._Shared.Services;
using Gym.Domain.AccountContext;
using Gym.Domain.ClientContext;
using MediatR;

namespace Gym.Application.Services.AccountApi.ChargeAccount
{
    internal class ChargeAccountHandler(
        IAccountRepository _accountRepository,
        IClientQueryService _clientQueryService,
        IChargeAccountService _chargeAccountService) : IRequestHandler<ChargeAccount, AccountDetails>
    {
        public async Task<AccountDetails> Handle(ChargeAccount request, CancellationToken cancellationToken)
        {
            ClientId clientId = ClientId.From(request.ClientId);
            Client? client = await _clientQueryService.GetByIdAsync(clientId, cancellationToken);
            if(client is null)
            {
                throw new ArgumentException($"Client id - {clientId} not exist");
            }

            AccountId accountId = AccountId.From(client.UserId);
            Account account = await _accountRepository.GetByIdAsync(accountId, cancellationToken);

            _chargeAccountService.ChargeAccount(account, request.ByCount);

            await _accountRepository.SaveAsync(account, cancellationToken);

            return account.ToDetails();
        }
    }
}
