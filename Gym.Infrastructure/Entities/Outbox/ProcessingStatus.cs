namespace Gym.Infrastructure.Entities.Outbox
{
    internal enum ProcessingStatus
    {
        Created,
        Processed,
        Failed,
        PendingRecovery,
        DeadLetter
    }
}
