namespace Gym.Domain._Common
{
    public interface IExclusiveAccessCoordinator
    {
        Task<ExclusiveAccessResult> TryAcquireAsync(String aggregateId, String operation, CancellationToken cancellationToken = default);
        Task<Boolean> ReleaseAsync(String aggregateId, String operation, String accessKey, CancellationToken cancellationToken = default);
    }

    public record ExclusiveAccessResult(Boolean Result, String? AccessKey)
    {
        public static ExclusiveAccessResult Successful(String accessKey) => new(true, accessKey);
        public static ExclusiveAccessResult Denied() => new(false, default);
    }
}
