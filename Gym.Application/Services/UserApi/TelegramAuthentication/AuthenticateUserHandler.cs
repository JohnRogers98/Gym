using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.AccountContext;
using Gym.Domain.AccountContext.ValueObjects;
using Gym.Domain.ClientContext;
using Gym.Domain.ClientContext.Errors;
using Gym.Domain.TelegramAuthContext;
using Gym.Domain.UserContext;
using Gym.Domain.UserContext.ValueObjects;
using MediatR;

namespace Gym.Application.Services.UserApi.TelegramAuthentication
{
    internal class AuthenticateUserHandler(
        ITelegramSignatureVerifier _telegramSignatureVerifier,
        IUserRepository _userRepository,
        ITelegramAuthRepository _telegramAuthRepository,
        IClientByUserIdFinder _clientByUserIdFinder,
        IClientRepository _clientRepository,
        IAccountRepository _accountRepository) : IRequestHandler<AuthenticateUser, Result<AuthenticatedUserDetails>>
    {
        public async Task<Result<AuthenticatedUserDetails>> Handle(AuthenticateUser request, CancellationToken cancellationToken)
        {
            Result<ValidatedTelegramUserInfo> verificationResult = _telegramSignatureVerifier.Verify(request.EscapedInitData);
            if (verificationResult.Success is false)
                return Result<AuthenticatedUserDetails>.Fail(verificationResult.Error!);

            Boolean wasRegistered = await _telegramAuthRepository.ExistsAsync(verificationResult.Data!.Id, cancellationToken);
            if(wasRegistered is false)
            {
                await RegisterUser(verificationResult.Data!, cancellationToken);
            }

            TelegramAuth? telegramAuth = await _telegramAuthRepository.GetByIdAsync(verificationResult.Data!.Id, cancellationToken);

            Client? client = await _clientByUserIdFinder.GetByUserIdAsync(telegramAuth!.UserId, cancellationToken);
            if (client is null)
                return Result<AuthenticatedUserDetails>.Fail(ClientNotFoundByUserIdError.Create(telegramAuth.UserId));

            User? user = await _userRepository.GetByIdAsync(telegramAuth.UserId, cancellationToken);

            return Result<AuthenticatedUserDetails>.Ok(new AuthenticatedUserDetails(telegramAuth.UserId.Value, user!.Role.ToString()));
        }

        private async Task RegisterUser(ValidatedTelegramUserInfo validatedTelegramUserInfo, CancellationToken cancellationToken)
        {
            UserId userId = _userRepository.NextIdentity();
            User newUser = User.Create(
                userId,
                UserRole.Client,
                validatedTelegramUserInfo.FirstName,
                validatedTelegramUserInfo.LastName
            );

            await _userRepository.SaveAsync(newUser, cancellationToken);

            await this.CreateTelegramAuthForRegisteredUser(userId, validatedTelegramUserInfo, cancellationToken);
            await this.CreateClientForRegisteredUser(userId, cancellationToken);
            await this.CreateAccountForRegisteredUser(userId, cancellationToken);
        }

        private async Task<TelegramAuth> CreateTelegramAuthForRegisteredUser(UserId registeredUserId, ValidatedTelegramUserInfo validatedTelegramUserInfo, CancellationToken cancellationToken)
        {
            TelegramAuth telegramAuth = TelegramAuth.Create(
                validatedTelegramUserInfo.Id,
                registeredUserId,
                validatedTelegramUserInfo.Username);
            
            await _telegramAuthRepository.SaveAsync(telegramAuth, cancellationToken);
            return telegramAuth;
        }

        private async Task<Client> CreateClientForRegisteredUser(UserId registeredUserId, CancellationToken cancellationToken)
        {
            Client client = Client.Create(_clientRepository.NextIdentity(), registeredUserId);
            await _clientRepository.SaveAsync(client, cancellationToken);
            return client;
        }

        private async Task<Account> CreateAccountForRegisteredUser(UserId registeredUserId, CancellationToken cancellationToken)
        {
            Account account = Account.Create(AccountId.From(registeredUserId), registeredUserId);
            await _accountRepository.SaveAsync(account, cancellationToken);
            return account;
        }

    }
}