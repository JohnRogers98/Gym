using Gym.WebApplication.Operations;

namespace Gym.WebApplication.Features._Common.Services
{
    public class HttpExceptionCatcherDecorator<TRequest, TResponse>(
        IRequestHandler<TRequest, TResponse> _decoratee) : IRequestHandler<TRequest, TResponse>, IRequestHandlerDecoratorMarker
    {
        public async Task<AsyncOperation<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _decoratee.HandleAsync(request, cancellationToken);
            }
            catch (HttpRequestException)
            {
                return AsyncOperation<TResponse>.Failure("Network error occured.", ErrorType.Network);
            }
        }
    }
}
