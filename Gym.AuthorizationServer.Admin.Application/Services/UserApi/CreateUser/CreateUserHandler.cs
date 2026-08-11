using Gym.AuthorizationServer.Admin.Application.Abstractions;
using Gym.AuthorizationServer.Infrastructure.Entities.Roles;
using Gym.AuthorizationServer.Infrastructure.Entities.Users;
using Gym.AuthorizationServer.Infrastructure.Entities.Users.FormCredentials;
using Gym.AuthorizationServer.Infrastructure.Services;
using Gym.RabbitMQ.Topology.Messages;
using Gym.RabbitMQ.Topology.Services;
using MediatR;

namespace Gym.AuthorizationServer.Admin.Application.Services.UserApi.CreateUser
{
    internal class CreateUserHandler(
        IUserRepository _userRepository,
        IRoleRepository _roleRepository,
        IFormCredentialRepository _formCredentialRepository,
        IPasswordHasher _passwordHasher,
        IUserCreatedEventService _userCreatedEventService) : IRequestHandler<CreateUser, Result<CreateUserResult>>
    {
        public async Task<Result<CreateUserResult>> Handle(CreateUser request, CancellationToken cancellationToken)
        {
            UserRoleEntity? userRole = await _roleRepository.GetByIdAsync(request.RoleId, cancellationToken);
            if (userRole is null)
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

            UserCreatedMessage userCreatedMessage = new()
            {
                UserId = userEntity.Id,
                Role = userRole.Name,
                FirstName = userEntity.FirstName,
                LastName= userEntity.LastName,
            };
            await _userCreatedEventService.PublishAsync(userCreatedMessage, cancellationToken);

            return Result<CreateUserResult>.Success(new(userEntity.Id));
        }
    }
}
