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
        IClientByUserIdFinder _clientByUserIdFinder,
        IClientRepository _clientRepository,
        IAccountRepository _accountRepository) : IRequestHandler<AuthenticateUser, AuthenticatedUserDetails>
    {
        public async Task<AuthenticatedUserDetails> Handle(AuthenticateUser request, CancellationToken cancellationToken)
        {
            Result<ValidatedTelegramUserInfo> verificationResult = _telegramSignatureVerifier.Verify(request.EscapedInitData);
            if (!verificationResult.Success)
                throw new ArgumentException(verificationResult.Error!.GetErrorMessage());

            User? user = await _userByTelegramIdFinder.GetByTelegramIdAsync(verificationResult.Data!.Id, cancellationToken);
            if (user is null)
            {
                user = await this.RegisterUser(verificationResult.Data.Id, cancellationToken);
            }

            Client client = await _clientByUserIdFinder.GetByUserIdAsync(user.Id, cancellationToken)
                    ?? throw new ArgumentNullException();

            return new AuthenticatedUserDetails(user.Id.Value, client.Id.Value, user.Role.ToString(), user.TelegramId?.Value);
        }

        private async Task<User> RegisterUser(TelegramId telegramId, CancellationToken cancellationToken)
        {
            UserId userId = _userRepository.NextIdentity();
            User newUser = User.Create(userId, UserRole.Client, telegramId);
            await _userRepository.SaveAsync(newUser, cancellationToken);

            await this.CreateClientFromRegisteredUser(userId, cancellationToken);
            await this.CreateAccountFromRegisteredUser(userId, cancellationToken);

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