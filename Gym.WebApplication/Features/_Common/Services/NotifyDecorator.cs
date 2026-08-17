using Gym.WebApplication.Operations;

namespace Gym.WebApplication.Features._Common.Services
{
    public class NotifyDecorator<TRequest, TResponse>(
        IRequestHandler<TRequest, TResponse> _decoratee,
        AsyncOperationStateNotifier<TRequest, TResponse> _notifier) : IRequestHandler<TRequest, TResponse>, IRequestHandlerDecoratorMarker
    {
        public async Task<AsyncOperation<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _decoratee.HandleAsync(request, cancellationToken);    
            _notifier.Notify(result);
            return result;
        }
    }
}
