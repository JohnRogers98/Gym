using Gym.WebApplication.Features._Common.States;
using Gym.WebApplication.Operations;

namespace Gym.WebApplication.Features._Common.Services
{
    public class FailSnackbarDecorator<TRequest, TResponse>(
        IRequestHandler<TRequest, TResponse> _decoratee,
        IAppSnackbarNotifier _appSnackbarNotifier) : IRequestHandler<TRequest, TResponse>
    {
        public async Task<AsyncOperation<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _decoratee.HandleAsync(request, cancellationToken);

            if(result.Succeeded is false)
            {
                _appSnackbarNotifier.ShowMessage(String.IsNullOrWhiteSpace(result.ErrorMessage) ? "Error" : $"{result.ErrorMessage}", MessageSeverity.Error);
            }

            return result;
        }
    }
}
