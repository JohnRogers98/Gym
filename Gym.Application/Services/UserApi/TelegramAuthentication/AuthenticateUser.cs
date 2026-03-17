using Gym.Application.Aspects;
using Gym.Domain._Common;
using MediatR;

namespace Gym.Application.Services.UserApi.TelegramAuthentication
{
    public record AuthenticateUser(String EscapedInitData) : IRequest<Result<AuthenticatedUserDetails>>, ITransactionalRequest;
}
