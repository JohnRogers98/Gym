using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.AccountContext;
using Gym.Domain.AccountContext.ValueObjects;
using Gym.Domain.ClientContext;
using Gym.Domain.ClientContext.ValueObjects;
using Gym.Domain.FormAuthContext;
using Gym.Domain.FormAuthContext.Errors;
using Gym.Domain.FormAuthContext.ValueObjects;
using Gym.Domain.UserContext;
using Gym.Domain.UserContext.ValueObjects;
using MediatR;

namespace Gym.Application.Services.ClientApi.CreateClient
{
    internal class CreateClientHandler(
        IUserRepository _userRepository,
        IClientRepository _clientRepository,
        IAccountRepository _accountRepository,
        IPasswordGenerator _passwordGenerator,
        IPasswordHasher _passwordHasher,
        IFormAuthRepository _formAuthRepository) : IRequestHandler<CreateClient, Result<CreateClientResult>>
    {
        public async Task<Result<CreateClientResult>> Handle(CreateClient request, CancellationToken cancellationToken)
        {  
            var userResult = await this.CreateUserAsync(request, cancellationToken);
            if(userResult.Success is false)
                return Result<CreateClientResult>.Fail(userResult.Error!);

            await CreateClientAsync(userResult.Data!.Id, cancellationToken);
            await CreateAccountAsync(userResult.Data!.Id, cancellationToken);

            var loginResult = Login.From(request.Login);
            if (loginResult.Success is false)
                return Result<CreateClientResult>.Fail(loginResult.Error!);

            var loginExists = await _formAuthRepository.ExistsAsync(loginResult.Data!, cancellationToken);
            if (loginExists)
                return Result<CreateClientResult>.Fail(LoginAlreadyExistsError.Create());

            Password password = _passwordGenerator.Generate();
            var hashedPassword = _passwordHasher.HashPassword(password);

            var formAuth = FormAuth.Create(loginResult.Data!, hashedPassword, userResult.Data!.Id);
            await _formAuthRepository.SaveAsync(formAuth, cancellationToken);

            return Result<CreateClientResult>.Ok(new CreateClientResult(userResult.Data!.Id.Value, loginResult.Data!.Value, password.Value));
        }

        private async Task<Result<User>> CreateUserAsync(CreateClient request, CancellationToken cancellationToken)
        {
            var userId = _userRepository.NextIdentity();

            var firstNameResult = FirstName.From(request.FirstName);
            if (firstNameResult.Success is false)
                return Result<User>.Fail(firstNameResult.Error!);

            LastName? lastName = null;
            if (request.LastName is not null)
            {
                var lastNameResult = LastName.From(request.LastName);
                if (lastNameResult.Success is false)
                    return Result<User>.Fail(lastNameResult.Error!);
                lastName = lastNameResult.Data!;
            }

            User user = User.Create(userId, UserRole.Client, firstNameResult.Data!, lastName);
            await _userRepository.SaveAsync(user, cancellationToken);

            return Result<User>.Ok(user);
        }

        private async Task<Client> CreateClientAsync(UserId userId, CancellationToken cancellationToken)
        {
            Client client = Client.Create(ClientId.From(userId), userId);
            await _clientRepository.SaveAsync(client, cancellationToken);
            return client;
        }

        private async Task<Account> CreateAccountAsync(UserId userId, CancellationToken cancellationToken)
        {
            Account account = Account.Create(AccountId.From(userId), userId);
            await _accountRepository.SaveAsync(account, cancellationToken);
            return account;
        }
    }
}
