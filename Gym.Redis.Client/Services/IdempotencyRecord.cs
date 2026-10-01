namespace Gym.Redis.Client.Services
{
    public record IdempotencyRecord
    {
        public required IdempotencyRecordState State { get; init; } = IdempotencyRecordState.InProgress;
        public Int32? StatusCode { get; init; }
        public String? ContentType { get; init; }
        public String? Location { get; init; }
        public String? ResponseBody { get; init; }
    }

    public enum IdempotencyRecordState
    {
        InProgress,
        Completed
    }

}
