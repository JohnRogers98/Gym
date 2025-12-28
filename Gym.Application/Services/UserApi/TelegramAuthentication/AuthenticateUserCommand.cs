using MediatR;

namespace Gym.Application.Services.UserApi.TelegramAuthentication
{
    public record AuthenticateUserCommand(String escapedInitData) : IRequest<UserDetails>;
}
