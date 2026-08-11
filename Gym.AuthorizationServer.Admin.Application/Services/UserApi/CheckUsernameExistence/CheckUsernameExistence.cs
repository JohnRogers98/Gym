using Gym.AuthorizationServer.Admin.Application.Abstractions;
using MediatR;

namespace Gym.AuthorizationServer.Admin.Application.Services.UserApi.CheckUsernameExistence
{
    public record CheckUsernameExistence(String Username) : IRequest<Result<Boolean>>;
}
