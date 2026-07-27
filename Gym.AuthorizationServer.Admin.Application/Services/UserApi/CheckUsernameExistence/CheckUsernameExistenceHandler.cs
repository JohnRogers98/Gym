using Gym.AuthorizationServer.Admin.Application.Abstractions;
using Gym.AuthorizationServer.Infrastructure.Entities.Users.FormCredentials;
using MediatR;

namespace Gym.AuthorizationServer.Admin.Application.Services.UserApi.CheckUsernameExistence
{
    internal class CheckUsernameExistenceHandler(IFormCredentialRepository _formCredentialRepository) : IRequestHandler<CheckUsernameExistence, Result<Boolean>>
    {
        public async Task<Result<Boolean>> Handle(CheckUsernameExistence request, CancellationToken cancellationToken)
        {
            var usernameExist = await _formCredentialRepository.ExistsByUsernameAsync(request.Username, cancellationToken);
            return Result<Boolean>.Success(usernameExist);
        }
    }
}
