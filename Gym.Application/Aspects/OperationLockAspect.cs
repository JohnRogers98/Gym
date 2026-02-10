using Gym.Domain._Common;
using MediatR;

namespace Gym.Application.Aspects
{
    internal class OperationLockAspect<TRequest, TResponse>(IExclusiveAccessCoordinator _exclusiveAccessCoordinator) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request is not ILockedRequest lockedRequest)
                return await next();

            ExclusiveAccessResult exclusiveAccessResult = await _exclusiveAccessCoordinator
                   .TryAcquireAsync(lockedRequest.GetLockId(), lockedRequest.GetLockOperation(), cancellationToken);
            if (exclusiveAccessResult.Result is false)
            {
                throw new Exception("Resource is under lock.");
            }
            try
            {
                return await next();
            }
            finally
            {
                await _exclusiveAccessCoordinator
                    .ReleaseAsync(lockedRequest.GetLockId(), lockedRequest.GetLockOperation(), exclusiveAccessResult.AccessKey!, cancellationToken);
            }
        }
    }
}
