using MediatR;

namespace Gym.AuthorizationServer.Admin.Application.Services.UserApi.GetAllUsers
{
    public record GetAllUsers : IRequest<IEnumerable<User>>;
}
