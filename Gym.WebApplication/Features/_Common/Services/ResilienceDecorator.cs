using Gym.WebApplication.Operations;
using Polly;
using Polly.Fallback;

namespace Gym.WebApplication.Features._Common.Services
{
    public class ResilienceDecorator<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    {
        private IRequestHandler<TRequest, TResponse> _decoratee;
        private ResiliencePipeline<AsyncOperation<TResponse>> _pipeline;

        public ResilienceDecorator(IRequestHandler<TRequest, TResponse> decoratee)
        {
            _decoratee = decoratee;

            _pipeline = new ResiliencePipelineBuilder<AsyncOperation<TResponse>>()
                .AddFallback(new FallbackStrategyOptions<AsyncOperation<TResponse>>
                {
                    FallbackAction = args => Outcome.FromResultAsValueTask(
                        AsyncOperation<TResponse>.Failure("Service unavailable", ErrorType.Timeout))
                })
                .AddTimeout(TimeSpan.FromSeconds(5))
                .AddRetry(new()
                {
                    MaxRetryAttempts = 3,
                    ShouldHandle = new PredicateBuilder<AsyncOperation<TResponse>>()
                        .Handle<HttpRequestException>()
                })
                .Build();
        }

        public async Task<AsyncOperation<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken = default)
        {
            return await _pipeline.ExecuteAsync(async innerToken =>
            {
                return await _decoratee.HandleAsync(request, innerToken);
            }, cancellationToken);
        }
    }
}
