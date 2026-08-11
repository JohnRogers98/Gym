using Gym.AuthorizationServer.Infrastructure.Session;

namespace Gym.AuthorizationServer.Extensions
{
    public static class MiddlewareExtensions
    {
        extension(WebApplication app)
        {
            public IApplicationBuilder UseCacheControlHeader()
            {
                return app.Use(async (context, next) =>
                {
                    context.Response.OnStarting(async () =>
                    {
                        var path = context.Request.Path.Value?.ToLowerInvariant();
                        if (path == "/token" || path == "/userinfo")
                        {
                            context.Response.Headers.CacheControl = "no-store";
                        }
                    });

                    await next();
                });
            }

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
}
