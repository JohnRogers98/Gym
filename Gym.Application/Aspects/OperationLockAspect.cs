using Gym.Domain._Common;
using MediatR;
using System.Reflection;

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
                return this.FailOperation();
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

        private TResponse FailOperation()
        {
            if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
            {
                Type valueType = typeof(TResponse).GetGenericArguments()[0];

                Type resultType = typeof(Result<>).MakeGenericType(valueType);

                MethodInfo? failMethod = resultType.GetMethod(nameof(Result<>.Fail),
                    BindingFlags.Static | BindingFlags.Public,
                    new[] { typeof(DomainError) });

                if (failMethod is null)
                    throw new InvalidOperationException($"Cannot find Fail method on {resultType}");

                Object? failure = failMethod.Invoke(null, new[] { ExclusiveAccessError.Create() });

                return (TResponse)failure!;
            }

            else if (typeof(TResponse) == typeof(Result))
                return (TResponse)(Object)Result.Fail(ExclusiveAccessError.Create());

            else
                throw new InvalidOperationException("Resource is under lock.");
        }

    }
}
