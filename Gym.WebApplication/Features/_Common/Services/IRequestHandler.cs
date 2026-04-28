using Gym.WebApplication.Operations;

namespace Gym.WebApplication.Features._Common.Services
{
    public interface IRequestHandler<TRequest, TResponse>
    {
        Task<AsyncOperation<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
    }
}
