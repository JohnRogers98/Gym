using Gym.Domain._Common;
using MediatR;

namespace Gym.Application.Services.UserApi.FormAuthentication
{
    public record AuthenticateUser(String Login, String Password) : IRequest<Result<AuthenticatedUserDetails>>;
}
