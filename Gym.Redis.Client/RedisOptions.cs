using System.ComponentModel.DataAnnotations;

namespace Gym.Redis.Client
{
    public class RedisOptions
    {
        [Required]
        public String ConnectionString { get; set; } = "localhost:6379";

        public String? KeyPrefix { get; set; } = String.Empty;

        public TimeSpan SessionKeyTtl { get; set; } = TimeSpan.FromDays(1);

        public TimeSpan IdempotencyKeyTtl { get; set; } = TimeSpan.FromMinutes(2);

        public TimeSpan LockKeyExpiry { get; set; } = TimeSpan.FromSeconds(30);
        public TimeSpan LockWait { get; set; } = TimeSpan.FromSeconds(2);
        public TimeSpan LockRetry { get; set; } = TimeSpan.FromMilliseconds(500);
    }
}