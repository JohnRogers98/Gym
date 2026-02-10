using MediatR;
using Microsoft.Extensions.Logging;

namespace Gym.Application.Aspects
{
    internal class LogAspect<TRequest, TResponse>(ILogger<LogAspect<TRequest,TResponse>> _logger) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            Guid logId = Guid.NewGuid();
            _logger.LogInformation($"LogId - {logId}: {typeof(TRequest).Name} is executed. {request.ToString()}");

            var result = await next();

            if (result is not null)
            {
                _logger.LogInformation($"LogId - {logId}: {typeof(TResponse).Name} is returned. {result.ToString()}");
            }

            return result;
        }
    }
}
