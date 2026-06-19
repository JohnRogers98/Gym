using Gym.AuthorizationServer.Admin.Application.Abstractions;
using Gym.AuthorizationServer.Infrastructure.Entities.Roles;
using Gym.AuthorizationServer.Infrastructure.Entities.Users;
using Gym.AuthorizationServer.Infrastructure.Entities.Users.FormCredentials;
using Gym.AuthorizationServer.Infrastructure.Services;
using MediatR;

namespace Gym.AuthorizationServer.Admin.Application.Services.UserApi.CreateUser
{
    internal class CreateUserHandler(
        IUserRepository _userRepository,
        IRoleRepository _roleRepository,
        IFormCredentialRepository _formCredentialRepository,
        IPasswordHasher _passwordHasher) : IRequestHandler<CreateUser, Result<CreateUserResult>>
    {
        public async Task<Result<CreateUserResult>> Handle(CreateUser request, CancellationToken cancellationToken)
        {
            Boolean roleExists = await _roleRepository.ExistsAsync(request.RoleId, cancellationToken);
            if (roleExists is false)
                return Result<CreateUserResult>.Failure("invalid_request", "Such role not exists");

            var suchUsernameExists = await _formCredentialRepository.ExistsByUsernameAsync(request.Username, cancellationToken);
            if(suchUsernameExists)
                return Result<CreateUserResult>.Failure("username_exists", "Such username already registered");

            UserEntity userEntity = new() 
            { 
                FirstName = request.FirstName,
                LastName = request.LastName,
                RoleId = request.RoleId 
            };
            await _userRepository.AddAsync(userEntity, cancellationToken);

            FormCredentialEntity formCredential = new()
            {
                Username = request.Username,
                HashedPassword = _passwordHasher.HashPassword(request.Password),
                UserId = userEntity.Id 
            };
            await _formCredentialRepository.AddAsync(formCredential, cancellationToken);

            return Result<CreateUserResult>.Success(new(userEntity.Id));
        }
    }
}
