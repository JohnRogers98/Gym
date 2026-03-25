using Gym.Domain._Common;
using MediatR;

namespace Gym.Application.Aspects
{
    internal class UnitOfWorkAspect<TRequest, TResponse>(IUnitOfWork _unitOfWork) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request is not ITransactionalRequest)
            {
                return await next();
            }
    
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var result = await next();

                if (this.IsSuccessfulResultReturned(result))
                    await _unitOfWork.CommitAsync(cancellationToken);

                else
                    await _unitOfWork.RollbackAsync(cancellationToken);

                return result;
            }
            catch
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                throw;
            }
        }

        private Boolean IsSuccessfulResultReturned(TResponse response)
        {
            if(response == null)
                return true;

            if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
            {
                dynamic result = response;
                return result.Success;
            }

            else if (typeof(TResponse) == typeof(Result))
            {
                Result result = (Result)(Object)response;
                return result.Success;
            }

            return true;
        }
    }
}
