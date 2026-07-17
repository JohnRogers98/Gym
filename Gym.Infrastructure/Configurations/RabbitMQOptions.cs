namespace Gym.Infrastructure.Configurations
{
    public sealed record RabbitMQOptions(String Hostname, String Username, String Password, String Exchange)
    {
        public static RabbitMQOptions Default => new RabbitMQOptions("localhost", "guest", "guest", String.Empty);
    }
}
