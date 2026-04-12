using Gym.Domain._Common;
using Gym.Domain.ClientContext.Errors;
using Gym.Domain.FormAuthContext;
using Gym.Domain.FormAuthContext.ValueObjects;
using Gym.Domain.UserContext;
using MediatR;

namespace Gym.Application.Services.UserApi.FormAuthentication
{
    internal class AuthenticateUserHandler(IFormAuthRepository _formAuthRepository, IUserRepository _userRepository, IPasswordHashValidator _passwordHashValidator) : IRequestHandler<AuthenticateUser, Result<AuthenticatedUserDetails>>
    {
        public async Task<Result<AuthenticatedUserDetails>> Handle(AuthenticateUser request, CancellationToken cancellationToken)
        {
            var loginResult = Login.From(request.Login);
            if (loginResult.Success is false)
                return Result<AuthenticatedUserDetails>.Fail(loginResult.Error!);

            FormAuth? formAuth = await _formAuthRepository.GetByLoginAsync(loginResult.Data!, cancellationToken);
            if (formAuth is null)
                return Result<AuthenticatedUserDetails>.Fail(SuchLoginNotExistsError.Create(loginResult.Data!));

            var passwordResult = Password.From(request.Password);
            if (passwordResult.Success is false)
                return Result<AuthenticatedUserDetails>.Fail(passwordResult.Error!);

            var passwordValidationResult = _passwordHashValidator.ValidateHash(formAuth.Password, passwordResult.Data!);
            if(passwordValidationResult.Success is false)
                return Result<AuthenticatedUserDetails>.Fail(passwordValidationResult.Error!);

            User? user = await _userRepository.GetByIdAsync(formAuth.UserId, cancellationToken);

            return Result<AuthenticatedUserDetails>.Ok(new AuthenticatedUserDetails(user!.Id.Value, user!.Role.ToString()));
        }
    }
}
