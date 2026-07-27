namespace Gym.RabbitMQ.Topology.Messages
{
    public record UserCreatedMessage
    {
        public required String UserId { get; init; }
        public required String Role { get; init; }
        public String? FirstName { get; init; }
        public String? LastName { get; init; }
        public Int64? TelegramId { get; init; }
    }
}
