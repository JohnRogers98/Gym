namespace Gym.Domain._Common
{
    public interface IExclusiveAccessCoordinator
    {
        Task<ExclusiveAccessResult> TryAcquireAsync(String aggregateId, String operation, CancellationToken cancellationToken = default);
        Task<Boolean> ReleaseAsync(String aggregateId, String operation, String accessKey, CancellationToken cancellationToken = default);
    }

    public record ExclusiveAccessResult
    {
        public Boolean Result { get; }
        public String? AccessKey { get; }

        private ExclusiveAccessResult(Boolean result, String? accessKey)
        {
            Result = result;
            AccessKey = accessKey;
        }

        public static ExclusiveAccessResult Successful(String accessKey) => new(true, accessKey);
        public static ExclusiveAccessResult Denied() => new(false, default);
    }

    public class ExclusiveAccessError : DomainError
    {
        private ExclusiveAccessError() : base(nameof(ExclusiveAccessError)) { }

        public static ExclusiveAccessError Create() => new();

        public override String GetErrorMessage() => $"Exclusive access is denied.";
    }
}
