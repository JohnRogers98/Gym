using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;

namespace Gym.Infrastructure.Configurations
{
    public class ProxyOptions
    {
        [ConfigurationKeyName("PROXY_HOST")]
        [Required]
        public String Host { get; set; } = default!;

        [ConfigurationKeyName("PROXY_PORT")]
        [Required]
        public String Port { get; set; } = default!;

        [ConfigurationKeyName("PROXY_LOGIN")]
        [Required]
        public String Login { get; set; } = default!;

        [ConfigurationKeyName("PROXY_PASSWORD")]
        [Required]
        public String Password { get; set; } = default!;

        public override String ToString()
        {
            return $"Host: {Host}, Port: {Port}, Login: {Login}, Password: {Password}";
        }
    }
}