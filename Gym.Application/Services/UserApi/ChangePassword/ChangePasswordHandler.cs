using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.ClientContext.Errors;
using Gym.Domain.FormAuthContext;
using Gym.Domain.FormAuthContext.ValueObjects;
using MediatR;

namespace Gym.Application.Services.UserApi.ChangePassword
{
    internal class ChangePasswordHandler(
        IPasswordHasher _passwordHasher,
        IPasswordHashValidator _passwordHashValidator,
        IFormAuthByUserIdFinder _formAuthByUserIdFinder,
        IFormAuthRepository _formAuthRepository) : IRequestHandler<ChangePassword, Result<ChangePasswordResult>>
    {
        public async Task<Result<ChangePasswordResult>> Handle(ChangePassword request, CancellationToken cancellationToken)
        {
            var userIdResult = UserId.From(request.UserId);
            if (userIdResult.Success is false)
                return Result<ChangePasswordResult>.Fail(userIdResult.Error!);

            FormAuth? formAuth = await _formAuthByUserIdFinder.GetFormAuthByUserIdAsync(userIdResult.Data!, cancellationToken);
            if(formAuth is null)
                return Result<ChangePasswordResult>.Fail(ClientNotFoundByUserIdError.Create(userIdResult.Data!));

            var oldPasswordResult = Password.From(request.OldPassword);
            if(oldPasswordResult.Success is false)
                return Result<ChangePasswordResult>.Fail(oldPasswordResult.Error!);

            var oldPasswordValidationResult = _passwordHashValidator.ValidateHash(formAuth.Password, oldPasswordResult.Data!);
            if (oldPasswordValidationResult.Success is false)
                return Result<ChangePasswordResult>.Fail(oldPasswordValidationResult.Error!);

            var newPasswordResult = Password.From(request.NewPassword);
            if (newPasswordResult.Success is false)
                return Result<ChangePasswordResult>.Fail(newPasswordResult.Error!);

            HashedPassword newHashedPassword = _passwordHasher.HashPassword(newPasswordResult.Data!);
            formAuth.ChangePassword(newHashedPassword);

            await _formAuthRepository.SaveAsync(formAuth, cancellationToken);

            return Result<ChangePasswordResult>.Ok(new());
        }
    }
}
