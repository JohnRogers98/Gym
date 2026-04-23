using Gym.Domain._Common;
using Gym.Domain._Shared;
using Gym.Domain.FormAuthContext;
using Gym.Domain.FormAuthContext.Errors;
using Gym.Domain.FormAuthContext.ValueObjects;
using Gym.Domain.InstructorContext;
using Gym.Domain.InstructorContext.ValueObjects;
using Gym.Domain.UserContext;
using Gym.Domain.UserContext.ValueObjects;
using MediatR;

namespace Gym.Application.Services.InstructorApi.CreateInstructor
{
    internal class CreateInstructorHandler(
        IInstructorRepository _instructorRepository,
        IUserRepository _userRepository,
        IPasswordGenerator _passwordGenerator,
        IPasswordHasher _passwordHasher,
        IFormAuthRepository _formAuthRepository) : IRequestHandler<CreateInstructor, Result<CreateInstructorResult>>
    {
        public async Task<Result<CreateInstructorResult>> Handle(CreateInstructor request, CancellationToken cancellationToken)
        {
            var userResult = await this.CreateUserAsync(request, cancellationToken);
            if(userResult.Success is false)
                return Result<CreateInstructorResult>.Fail(userResult.Error!);

            Instructor instructor = Instructor.Create(InstructorId.From(userResult.Data!.Id), userResult.Data!.Id);
            await _instructorRepository.SaveAsync(instructor, cancellationToken);

            var loginResult = Login.From(request.Login);
            if (loginResult.Success is false)
                return Result<CreateInstructorResult>.Fail(loginResult.Error!);

            var loginExists = await _formAuthRepository.ExistsAsync(loginResult.Data!, cancellationToken);
            if (loginExists)
                return Result<CreateInstructorResult>.Fail(LoginAlreadyExistsError.Create());

            Password password = _passwordGenerator.Generate();
            var hashedPassword = _passwordHasher.HashPassword(password);

            var formAuth = FormAuth.Create(loginResult.Data!, hashedPassword, userResult.Data!.Id);
            await _formAuthRepository.SaveAsync(formAuth, cancellationToken);

            return Result<CreateInstructorResult>.Ok(new CreateInstructorResult(instructor.Id.Value, loginResult.Data!.Value, password.Value));
        }

        private async Task<Result<User>> CreateUserAsync(CreateInstructor request, CancellationToken cancellationToken)
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

            User user = User.Create(userId, UserRole.Instructor, firstNameResult.Data!, lastName);
            await _userRepository.SaveAsync(user, cancellationToken);

            return Result<User>.Ok(user);
        }

    }
}
