using Gym.AuthorizationServer.Infrastructure.Session;

namespace Gym.AuthorizationServer.Admin.Extensions;

public static class MiddlewareExtensions
{
    extension(WebApplication app) 
    {
        public IApplicationBuilder UseUnitOfWork()
        {
            return app.Use(async (context, next) =>
            {
                var unitOfWork = context.RequestServices.GetRequiredService<IUnitOfWork>();

                try
                {
                    await unitOfWork.BeginTransactionAsync();
                    await next();
                    await unitOfWork.CommitAsync();
                }
                catch
                {
                    await unitOfWork.RollbackAsync();
                    throw;
                }
            });
        }
    }
}
