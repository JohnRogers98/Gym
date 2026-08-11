using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace System;

internal static class IServiceProviderExtensions
{
    public static T GetRequiredOption<T>(this IServiceProvider serviceProvider) where T : class
    {
        return serviceProvider.GetRequiredService<IOptions<T>>().Value;
    }
}
