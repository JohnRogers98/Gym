using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.AccountContext;
using Gym.Domain.ClientContext;
using Gym.Domain.UserContext;
using Gym.Domain.UserContext.Authentication;
using MediatR;

namespace Gym.Application.Services.UserApi.TelegramAuthentication
{
    internal class AuthenticateUserHandler(
        ITelegramSignatureVerifier _telegramSignatureVerifier,
        IUserRepository _userRepository,
        IUserByTelegramIdFinder _userByTelegramIdFinder,
        IClientRepository _clientRepository,
        IAccountRepository _accountRepository) : IRequestHandler<AuthenticateUser, UserDetails>
    {
        public async Task<UserDetails> Handle(AuthenticateUser request, CancellationToken cancellationToken)
        {
            Result<ValidatedTelegramUserInfo> verificationResult = _telegramSignatureVerifier.Verify(request.EscapedInitData);
            if (!verificationResult.Success)
                throw new ArgumentException(verificationResult.Error!.GetErrorMessage());

            User? user = await _userByTelegramIdFinder.GetByTelegramIdAsync(verificationResult.Data!.Id, cancellationToken);

            if(user is null)
            {
                user = await this.RegisterUser(verificationResult.Data.Id, cancellationToken);
            }

            return user!.ToDetails();
        }

        private async Task<User> RegisterUser(TelegramId telegramId, CancellationToken cancellationToken)
        {
            UserId userId = _userRepository.NextIdentity();
            User newUser = User.Create(userId, UserRole.Client, telegramId);
            await _userRepository.SaveAsync(newUser, cancellationToken);

            Client registeredClient = await this.CreateClientFromRegisteredUser(userId, cancellationToken);
            Account registeredAccount = await this.CreateAccountFromRegisteredUser(userId, cancellationToken);

            return newUser;
        }

        private async Task<Client> CreateClientFromRegisteredUser(UserId registeredUserId, CancellationToken cancellationToken)
        {
            Client client = Client.Create(_clientRepository.NextIdentity(), registeredUserId);
            await _clientRepository.SaveAsync(client, cancellationToken);
            return client;
        }

        private async Task<Account> CreateAccountFromRegisteredUser(UserId registeredUserId, CancellationToken cancellationToken)
        {
            Account account = Account.Create(AccountId.From(registeredUserId), registeredUserId);
            await _accountRepository.SaveAsync(account, cancellationToken);
            return account;
        }

    }
}