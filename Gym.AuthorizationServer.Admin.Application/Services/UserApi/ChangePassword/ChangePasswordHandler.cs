using Gym.AuthorizationServer.Admin.Application.Abstractions;
using Gym.AuthorizationServer.Infrastructure.Entities.Users.FormCredentials;
using Gym.AuthorizationServer.Infrastructure.Services;
using MediatR;

namespace Gym.AuthorizationServer.Admin.Application.Services.UserApi.ChangePassword
{
    internal class ChangePasswordHandler(
        IFormCredentialRepository _formCredentialRepository,
        IPasswordHashValidator _passwordHashValidator,
        IPasswordHasher _passwordHasher) : IRequestHandler<ChangePassword, Result>
    {
        public async Task<Result> Handle(ChangePassword request, CancellationToken cancellationToken)
        {
            var formCredentials = await _formCredentialRepository.GetByUserIdAsync(request.UserId, cancellationToken);
            if (formCredentials is null)
                return Result.Failure($"Form credentials for userId - {request.UserId} not exist");

            Boolean isCurrentPasswordValid = _passwordHashValidator.ValidatePassword(formCredentials.HashedPassword, request.CurrentPassword);
            if(isCurrentPasswordValid is false)
                return Result.Failure($"Password is not valid");

            String newPasswordHash = _passwordHasher.HashPassword(request.NewPassword);
            await _formCredentialRepository.UpdatePasswordAsync(formCredentials.Id, newPasswordHash, cancellationToken);

            return Result.Success();
        }
    }
}
