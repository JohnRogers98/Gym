using Gym.Application.Aspects;
using Gym.Domain._Common;
using MediatR;

namespace Gym.Application.Services.UserApi.ChangePassword
{
    public record ChangePassword(String UserId, String OldPassword, String NewPassword) : IRequest<Result<ChangePasswordResult>>, ITransactionalRequest;
}
