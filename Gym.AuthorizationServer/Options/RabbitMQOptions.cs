using System.ComponentModel.DataAnnotations;

namespace Gym.AuthorizationServer.Options
{
    public class RabbitMQOptions
    {
        public const String SectionName = "RabbitMQ";

        [Required]
        public String Hostname { get; set; } = default!;

        [Required]
        public String Username { get; set; } = default!;

        [Required]
        public String Password { get; set; } = default!;

        [Required]
        public String Exchange { get; set; } = default!;
    }
}