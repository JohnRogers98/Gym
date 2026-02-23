using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Text;

namespace Gym.Application.Aspects
{
    internal class LogAspect<TRequest, TResponse>(ILogger<LogAspect<TRequest,TResponse>> _logger) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            Guid logId = Guid.NewGuid();
            _logger.LogInformation($"LogId - {logId}: {typeof(TRequest).Name} is being executed. {request.ToString()}");

            var result = await next();

            if (result is not null)
            {
                if (result is IEnumerable enumerable) 
                {
                    StringBuilder enumerableLog = new();
                    enumerableLog.Append($"LogId - {logId}: Collection of elemnts is returned:");
                    foreach (var item in enumerable)
                    {
                        enumerableLog.AppendLine($"\n {item}");
                    }
                    _logger.LogInformation(enumerableLog.ToString());
                }
                else
                {
                    _logger.LogInformation($"LogId - {logId}: {typeof(TResponse).Name} is returned. {result.ToString()}");
                }
                
            }

            return result;
        }
    }
}
