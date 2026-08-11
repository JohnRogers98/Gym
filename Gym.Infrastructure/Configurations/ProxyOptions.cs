using System.ComponentModel.DataAnnotations;

namespace Gym.Infrastructure.Configurations
{
    public sealed class ProxyOptions
    {
        [Required]
        public String Host { get; set; } = default!;

        [Required]
        public String Port { get; set; } = default!;

        [Required]
        public String Login { get; set; } = default!;

        [Required]
        public String Password { get; set; } = default!;

        public override String ToString()
        {
            return $"Host: {Host}, Port: {Port}, Login: {Login}, Password: {Password}";
        }
    }
}