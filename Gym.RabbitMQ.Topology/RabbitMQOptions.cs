using System.ComponentModel.DataAnnotations;

namespace Gym.RabbitMQ.Topology
{
    public class RabbitMQOptions
    {
        [Required]
        public String Hostname { get; set; } = "localhost";

        [Required]
        public String Username { get; set; } = "guest";

        [Required]
        public String Password { get; set; } = "guest";

        [Required]
        public String Vhost { get; set; } = "/";

        public String AuthorizationServerExchange { get; set; } = "auth-events-x";
        public String AuthorizationServerUserCreatedQueue { get; set; } = "user-created-queue";
        public String AuthorizationServerDeadLetterExchange { get; set; } = "auth-events-dlx";
        public String AuthorizationServerDeadLetterQueue { get; set; } = "auth-events-dlq";
    }
}