using Microsoft.Extensions.Configuration;

namespace Gym.Infrastructure;

public static class IConfigurationExtensions
{
    public static String GetRequiredConfiguration(this IConfiguration configuration, String path) 
    {
        return configuration[path] ?? throw new InvalidOperationException($"Required configuration key '{path}' is not configured.");
    }
}