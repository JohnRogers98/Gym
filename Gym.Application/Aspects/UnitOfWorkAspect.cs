using Gym.Domain._Common;
using MediatR;

namespace Gym.Application.Aspects
{
    internal class UnitOfWorkAspect<TRequest, TResponse>(IUnitOfWork _unitOfWork) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request is not ITransactionalRequest transactionalRequest)
            {
                return await next();
            }
    
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var result = await next();
                await _unitOfWork.CommitAsync(cancellationToken);
                return result;
            }
            catch
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
