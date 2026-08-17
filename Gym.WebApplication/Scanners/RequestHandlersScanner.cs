using Gym.WebApplication.Features._Common.Services;
using System.Reflection;

namespace Gym.WebApplication.Scanners;

public static class RequestHandlersScanner
{
    public static void ScanAssembly(Assembly assembly, IServiceCollection services)
    {
        IEnumerable<Type> foundedRequestHanlderImplementors = ScanForRequestHandlers(assembly);

        foreach (var aRequestHandler in foundedRequestHanlderImplementors)
        {
            RegisterRequestHandler(services, aRequestHandler);
        }
    }

    private static IEnumerable<Type> ScanForRequestHandlers(Assembly assembly)
    {
         return assembly.GetTypes()
            .Where(type => !type.IsAbstract && !type.IsInterface)
            .Where(type => !type.IsGenericTypeDefinition)
            .Where(IsTypeImplementRequestHandler)
            .Where(type => IsTypeImplementDecoratorMarker(type) is false);
    }

    private static Boolean IsTypeImplementRequestHandler(Type type)
        => type.GetInterfaces().Any(@interface => @interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));

    private static IEnumerable<Type> GetImplementedRequestHandlerInterfaces(Type type)
        => type.GetInterfaces().Where(@interface => @interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));

    private static Boolean IsTypeImplementDecoratorMarker(Type type) 
        => typeof(IRequestHandlerDecoratorMarker).IsAssignableFrom(type);

    private static void RegisterRequestHandler(IServiceCollection services, Type requestHandlerImplementor)
    {
        var implementedInterfaces = GetImplementedRequestHandlerInterfaces(requestHandlerImplementor);

        foreach (var anImplementedInterface in implementedInterfaces)
        {
            var requestType = anImplementedInterface.GetGenericArguments()[0];
            var responseType = anImplementedInterface.GetGenericArguments()[1];

            var method = RegisterHandlerMethod.MakeGenericMethod(requestType, responseType, requestHandlerImplementor);
            method?.Invoke(null, new object[] { services });
        }
    }

    private static readonly MethodInfo RegisterHandlerMethod = 
        typeof(RequestHandlersScanner)
        .GetMethod(nameof(ProxyRegisterRequestHandler), BindingFlags.NonPublic | BindingFlags.Static)!;

    private static void ProxyRegisterRequestHandler<TRequest, TResponse, THandler>(IServiceCollection services) 
        where THandler : class, IRequestHandler<TRequest, TResponse>
    {
        RequestHandlerRegistration
            .WithRequest<TRequest>
            .WithResponse<TResponse>
            .For<THandler>
            .In(services)
            .DecorateWithResilience()
            .DecorateWithHttpExceptionCatcher()
            .DecorateWithFailSnackbar()
            .DecorateWithNotifier();
    }

}
