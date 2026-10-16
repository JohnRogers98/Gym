using Microsoft.AspNetCore.SignalR.Client;

namespace Gym.WebApplication.Options
{
    public sealed class ChatWSForeverRetryPolicy : IRetryPolicy
    {
        public TimeSpan? NextRetryDelay(RetryContext retryContext)
        {
            return retryContext.PreviousRetryCount switch
            {
                0 => TimeSpan.Zero,
                1 => TimeSpan.FromSeconds(2),
                _ => TimeSpan.FromSeconds(5)
            };
        }
    }
}
