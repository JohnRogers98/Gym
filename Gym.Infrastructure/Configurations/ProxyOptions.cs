using Microsoft.Extensions.Configuration;

namespace Gym.Infrastructure.Configurations
{
    public class ProxyOptions
    {
        [ConfigurationKeyName("PROXY_HOST")]
        public String Host { get; set; } = default!;

        [ConfigurationKeyName("PROXY_PORT")]
        public String Port { get; set; } = default!;

        [ConfigurationKeyName("PROXY_LOGIN")]
        public String Login { get; set; } = default!;

        [ConfigurationKeyName("PROXY_PASSWORD")]
        public String Password { get; set; } = default!;

        public override String ToString()
        {
            return $"Host: {Host}, Port: {Port}, Login: {Login}, Password: {Password}";
        }
    }
}