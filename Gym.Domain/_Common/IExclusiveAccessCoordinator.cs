namespace Gym.Domain._Common
{
    public interface IExclusiveAccessCoordinator
    {
        Task<ExclusiveAccessResult> TryAcquireAsync(String aggregateId, String operation, CancellationToken cancellationToken = default);
        Task<Boolean> ReleaseAsync(String aggregateId, String operation, Guid accessKey, CancellationToken cancellationToken = default);
    }

    public record ExclusiveAccessResult(Boolean result, Guid? accessId)
    {
        public static ExclusiveAccessResult Successful(Guid accessId) => new(true, accessId);
        public static ExclusiveAccessResult Denied(Guid accessId) => new(false, default);
    }
}
