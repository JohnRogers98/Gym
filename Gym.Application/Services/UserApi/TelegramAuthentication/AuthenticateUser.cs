using Gym.Application.Aspects;
using MediatR;

namespace Gym.Application.Services.UserApi.TelegramAuthentication
{
    public record AuthenticateUser(String EscapedInitData) : IRequest<UserDetails>, ITransactionalRequest;
}
