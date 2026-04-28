using Gym.WebApplication.Features._Common.Services;

namespace Gym.WebApplication.Operations
{
    public class RequestHandlerRegistration
    {
        
        public class WithRequest<TRequest>
        {
            
            public class WithResponse<TResponse>
            {
            
                public class For<TService> where TService : class, IRequestHandler<TRequest, TResponse>
                {
                    private IServiceCollection _services;

                    private For(IServiceCollection services)
                    {
                        _services = services;
                    }

                    public static For<TService> In(IServiceCollection services)
                    {
                        services.AddScoped<IRequestHandler<TRequest, TResponse>, TService>();

                        return new For<TService>(services);
                    }

                    public For<TService> DecorateWithFailSnackbar()
                    {
                        _services.Decorate<IRequestHandler<TRequest, TResponse>, FailSnackbarDecorator<TRequest, TResponse>>();
                        return this;
                    }

                    public For<TService> DecorateWithHttpExceptionCatcher()
                    {
                        _services.Decorate<IRequestHandler<TRequest, TResponse>, HttpExceptionCatcherDecorator<TRequest, TResponse>>();
                        return this;
                    }

                    public For<TService> DecorateWithResilience()
                    {
                        _services.Decorate<IRequestHandler<TRequest, TResponse>, ResilienceDecorator<TRequest, TResponse>>();
                        return this;
                    }
                }

            }
        }
    }

}
