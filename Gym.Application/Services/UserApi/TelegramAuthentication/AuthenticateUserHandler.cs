using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.AccountContext;
using Gym.Domain.AccountContext.ValueObjects;
using Gym.Domain.ClientContext;
using Gym.Domain.ClientContext.Errors;
using Gym.Domain.UserContext;
using Gym.Domain.UserContext.Authentication;
using Gym.Domain.UserContext.ValueObjects;
using MediatR;

namespace Gym.Application.Services.UserApi.TelegramAuthentication
{
    internal class AuthenticateUserHandler(
        ITelegramSignatureVerifier _telegramSignatureVerifier,
        IUserRepository _userRepository,
        IUserByTelegramIdFinder _userByTelegramIdFinder,
        IClientByUserIdFinder _clientByUserIdFinder,
        IClientRepository _clientRepository,
        IAccountRepository _accountRepository) : IRequestHandler<AuthenticateUser, Result<AuthenticatedUserDetails>>
    {
        public async Task<Result<AuthenticatedUserDetails>> Handle(AuthenticateUser request, CancellationToken cancellationToken)
        {
            Result<ValidatedTelegramUserInfo> verificationResult = _telegramSignatureVerifier.Verify(request.EscapedInitData);
            if (verificationResult.Success is false)
                return Result<AuthenticatedUserDetails>.Fail(verificationResult.Error!);

            User? user = await _userByTelegramIdFinder.GetByTelegramIdAsync(verificationResult.Data!.Id, cancellationToken);
            if (user is null)
            {
                user = await this.RegisterUser(verificationResult.Data, cancellationToken);
            }

            Client? client = await _clientByUserIdFinder.GetByUserIdAsync(user.Id, cancellationToken);
            if (client is null)
                return Result<AuthenticatedUserDetails>.Fail(ClientNotFoundByUserIdError.Create(user.Id));

            return Result<AuthenticatedUserDetails>.Ok(new AuthenticatedUserDetails(user.Id.Value, client.Id.Value, user.Role.ToString(), user.TelegramId?.Value));
        }

        private async Task<User> RegisterUser(ValidatedTelegramUserInfo validatedTelegramUserInfo, CancellationToken cancellationToken)
        {
            UserId userId = _userRepository.NextIdentity();
            User newUser = User.Create(
                userId,
                UserRole.Client,
                validatedTelegramUserInfo.Id,
                validatedTelegramUserInfo.Username,
                validatedTelegramUserInfo.FirstName,
                validatedTelegramUserInfo.LastName
            );

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