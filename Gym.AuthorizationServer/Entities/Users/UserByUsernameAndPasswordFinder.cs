using Gym.AuthorizationServer.Entities.Users.FormCredentials;
using Gym.AuthorizationServer.Services;
using Gym.AuthorizationServer.Shared.Abstractions;

namespace Gym.AuthorizationServer.Entities.Users
{
    public interface IUserByUsernameAndPasswordFinder
    {
        Task<Result<UserEntity>> FindAsync(String username, String password, CancellationToken cancellationToken);
    }

    public class UserByUsernameAndPasswordFinder(
        IFormCredentialRepository _formCredentialRepository,
        IUserRepository _userRepository,
        IPasswordHashValidator _passwordHashValidator) : IUserByUsernameAndPasswordFinder
    {
        public async Task<Result<UserEntity>> FindAsync(String username, String password, CancellationToken cancellationToken)
        {
            FormCredentialEntity? formCredential = await _formCredentialRepository.GetByUsernameAsync(username, cancellationToken);
            if (formCredential is null)
                return Result<UserEntity>.Failure("invalid_credentials", "No such username exist");

            Boolean passwordValid = _passwordHashValidator.ValidatePassword(formCredential.HashedPassword, password);
            if (passwordValid is false)
                return Result<UserEntity>.Failure("invalid_credentials", "Password not valid");

            UserEntity? user = await _userRepository.GetByIdAsync(formCredential.UserId, cancellationToken);
            if(user is null)    
                return Result<UserEntity>.Failure("data_inconsistency", "User data is inconsistent");

            return Result<UserEntity>.Success(user);
        }
    }
}
