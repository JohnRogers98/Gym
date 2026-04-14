using Gym.Infrastructure.Entities.Outbox;
using Gym.Infrastructure.Entities.Outbox.Readers;
using Gym.Infrastructure.Entities.Outbox.Updaters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Gym.Infrastructure.HostedServices
{
    internal class OutboxDeadLetterRevoker(
        IOutboxReader _outboxReader,
        [FromKeyedServices(nameof(OutboxDeadLetterRevoker))] PeriodicTimer _periodicTimer,
        IServiceScopeFactory _serviceLocator) : BackgroundService
    {

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using PeriodicTimer periodicTimer = _periodicTimer;

            while (await periodicTimer.WaitForNextTickAsync(stoppingToken))
            {
                await RecoverStalledOutboxMessagesAsync(stoppingToken);
            }
        }

        private async Task RecoverStalledOutboxMessagesAsync(CancellationToken cancellationToken)
        {
            var failedMessages = await _outboxReader.GetFailedMessagesAsync(cancellationToken);

            foreach (var aFailedMessage in failedMessages)
            {
                await using var scope = _serviceLocator.CreateAsyncScope();
                IServiceProvider serviceProvider = scope.ServiceProvider;

                var outboxMessageStatusUpdater = serviceProvider.GetRequiredService<IOutboxMessageStatusUpdater>();
                await outboxMessageStatusUpdater.UpdateMessageStatusAsync(aFailedMessage.Id, ProcessingStatus.PendingRecovery, cancellationToken);
            }
        }
    }
}
