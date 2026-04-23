using Gym.Application.Aspects;
using Gym.Domain._Common;
using MediatR;

namespace Gym.Application.Services.ClientApi.CreateClient
{
    public record CreateClient(String Login, String FirstName, String? LastName) : IRequest<Result<CreateClientResult>>, ITransactionalRequest;
}
