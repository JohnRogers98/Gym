using Gym.Application.Aspects;
using Gym.Domain._Common;
using MediatR;

namespace Gym.Application.Services.UserApi.CreateClient
{
    public record CreateUser(String UserId, String Role, String FirstName, String? LastName, Int64? TelegramId) : IRequest<Result>, ITransactionalRequest;
}
