using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.AccountContext;
using Gym.Domain.AccountContext.ValueObjects;
using Gym.Domain.ClientContext;
using Gym.Domain.ClientContext.ValueObjects;
using Gym.Domain.InstructorContext;
using Gym.Domain.InstructorContext.ValueObjects;
using Gym.Domain.UserContext;
using Gym.Domain.UserContext.Errors;
using Gym.Domain.UserContext.ValueObjects;
using MediatR;

namespace Gym.Application.Services.UserApi.CreateClient
{
    internal class CreateUserHandler(
        IUserRepository _userRepository,
        IClientRepository _clientRepository,
        IAccountRepository _accountRepository,
        IInstructorRepository _instructorRepository) : IRequestHandler<CreateUser, Result>
    {
        public async Task<Result> Handle(CreateUser request, CancellationToken cancellationToken)
        {
            var createdUserResult = await this.CreateUserAsync(request, cancellationToken);
            if (createdUserResult.Success is false)
                return Result.Fail(createdUserResult.Error!);

            switch (createdUserResult.Data!.Role)
            {
                case UserRole.Client:
                    {
                        var createClientResult = await this.CreateClientAsync(createdUserResult.Data!.Id, cancellationToken);
                        if (createClientResult.Success is false)
                            return Result.Fail(createClientResult.Error!);

                        var createAccountResult = await this.CreateAccountAsync(createdUserResult.Data!.Id, cancellationToken);
                        if (createAccountResult.Success is false)
                            return Result.Fail(createAccountResult.Error!);

                        break;
                    }
                case UserRole.Instructor:
                    {
                        var createInstructorResult = await this.CreateInstructorAsync(createdUserResult.Data!.Id, cancellationToken);
                        if (createInstructorResult.Success is false)
                            return Result.Fail(createInstructorResult.Error!);

                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            return Result.Ok();
        }   

        private async Task<Result<User>> CreateUserAsync(CreateUser createUser, CancellationToken cancellationToken)
        {
            var userIdResult = UserId.From(createUser.UserId);
            if (userIdResult.Success is false)
                return Result<User>.Fail(userIdResult.Error!);

            var isUserRoleParsed = Enum.TryParse<UserRole>(createUser.Role, true, out var userRole);
            if (isUserRoleParsed is false)
                return Result<User>.Fail(UserRoleParseError.Create());

            var firstNameResult = FirstName.From(createUser.FirstName!);
            if (firstNameResult.Success is false)
                return Result<User>.Fail(firstNameResult.Error!);

            LastName? lastName = null;
            if (createUser.LastName is not null)
            {
                var lastNameResult = LastName.From(createUser.LastName);
                if (lastNameResult.Success is false)
                    return Result<User>.Fail(lastNameResult.Error!);
                lastName = lastNameResult.Data!;
            }

            TelegramId? telegramId = null;
            if (createUser.TelegramId is not null)
            {
                var telegramIdResult = TelegramId.From(createUser.TelegramId.Value);
                if (telegramIdResult.Success is false)
                    return Result<User>.Fail(telegramIdResult.Error!);
                telegramId = telegramIdResult.Data!;
            }

            User newUser = User.Create(userIdResult.Data!, userRole, firstNameResult.Data!, lastName, telegramId);
            await _userRepository.SaveAsync(newUser, cancellationToken);

            return Result<User>.Ok(newUser);
        }

        private async Task<Result<Client>> CreateClientAsync(UserId userId, CancellationToken cancellationToken)
        {
            Client client = Client.Create(ClientId.From(userId), userId);
            await _clientRepository.SaveAsync(client, cancellationToken);
            return Result<Client>.Ok(client);
        }

        private async Task<Result<Account>> CreateAccountAsync(UserId userId, CancellationToken cancellationToken)
        {
            Account account = Account.Create(AccountId.From(userId), userId);
            await _accountRepository.SaveAsync(account, cancellationToken);
            return Result<Account>.Ok(account);
        }

        private async Task<Result<Instructor>> CreateInstructorAsync(UserId userId, CancellationToken cancellationToken)
        {
            Instructor instructor = Instructor.Create(InstructorId.From(userId), userId);
            await _instructorRepository.SaveAsync(instructor, cancellationToken);
            return Result<Instructor>.Ok(instructor);
        }
    }
}
