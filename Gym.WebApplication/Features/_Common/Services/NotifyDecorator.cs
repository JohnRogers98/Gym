using Gym.WebApplication.Operations;

namespace Gym.WebApplication.Features._Common.Services
{
    public class NotifyDecorator<TRequest, TResponse>(
        IRequestHandler<TRequest, TResponse> _decoratee,
        AsyncOperationStateNotifier<TRequest, TResponse> _cacheInvalidator) : IRequestHandler<TRequest, TResponse>
    {
        public async Task<AsyncOperation<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _decoratee.HandleAsync(request, cancellationToken);    
            _cacheInvalidator.Notify(result);
            return result;
        }
    }
}
