using Gym.AuthorizationServer.Admin.Application.Abstractions;
using MediatR;

namespace Gym.AuthorizationServer.Admin.Application.Services.UserApi.ChangePassword
{
    public record ChangePassword(String UserId, String CurrentPassword, String NewPassword) : IRequest<Result>;
}
