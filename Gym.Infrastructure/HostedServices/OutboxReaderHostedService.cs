using Gym.Abstractions.MessageBus.Publishers;
using Gym.Domain._Common;
using Gym.Infrastructure.Entities.EventStores;
using Gym.Infrastructure.Entities.EventStores.Deserializers;
using Gym.Infrastructure.Entities.EventStores.Readers;
using Gym.Infrastructure.Entities.Outbox;
using Gym.Infrastructure.Entities.Outbox.Readers;
using Gym.Infrastructure.Entities.Outbox.Updaters;
using Gym.Infrastructure.Entities.Projections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace Gym.Infrastructure.HostedServices
{
    internal class OutboxReaderHostedService(
        IOutboxResumeTokenStore _outboxResumeTokenStore,
        IOutboxReader _outboxReader,
        IMongoCollection<MessageEntity> _messageCollection,
        IServiceScopeFactory _serviceLocator) : BackgroundService
    {

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (stoppingToken.IsCancellationRequested is false)
            {
                try
                {
                    await RecoverStalledOutboxMessagesAsync(stoppingToken);
                    await WatchIncomingOutboxMessagesAsync(stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }

        private async Task RecoverStalledOutboxMessagesAsync(CancellationToken cancellationToken)
        {
            var stalledMessages = await _outboxReader.GetStalledMessagesAsync(cancellationToken);

            foreach(var aStalledMessage in stalledMessages)
            {
                await this.HandleMessageAsync(aStalledMessage, cancellationToken);
            };
        }

        private async Task WatchIncomingOutboxMessagesAsync(CancellationToken cancellationToken)
        {
            ResumeToken? resumeToken = await _outboxResumeTokenStore.GetAsync(cancellationToken);

            var pipeline = new EmptyPipelineDefinition<ChangeStreamDocument<MessageEntity>>()
                .Match(change => 
                    (change.OperationType == ChangeStreamOperationType.Insert && change.FullDocument.Status == nameof(ProcessingStatus.Created))
                    || (change.OperationType == ChangeStreamOperationType.Update && change.FullDocument.Status == nameof(ProcessingStatus.PendingRecovery)));

            var options = new ChangeStreamOptions
            {
                FullDocument = ChangeStreamFullDocumentOption.UpdateLookup,
                StartAfter = resumeToken?.Value,
                BatchSize = 100
            };

            using var cursor = await _messageCollection.WatchAsync(pipeline, options, cancellationToken);

            while (await cursor.MoveNextAsync(cancellationToken))
            {
                foreach (var aChange in cursor.Current)
                {
                    await this.HandleMessageAsync(aChange.FullDocument, cancellationToken);
                    await _outboxResumeTokenStore.SaveAsync(ResumeToken.From(aChange.ResumeToken), cancellationToken);
                }
            }
        }

        private async Task HandleMessageAsync(MessageEntity outboxMessage, CancellationToken cancellationToken)
        {
            await using var scope = _serviceLocator.CreateAsyncScope();
            IServiceProvider serviceProvider = scope.ServiceProvider;

            var mongoUnitOfWork = serviceProvider.GetRequiredService<MongoUnitOfWork>();
            var outboxMessageStatusUpdater = serviceProvider.GetRequiredService<IOutboxMessageStatusUpdater>();
            try
            {
                await mongoUnitOfWork.BeginTransactionAsync(cancellationToken);

                EventEntity eventEntity = await this.GetEventEntityAsync(serviceProvider, outboxMessage, cancellationToken);
                await this.RunProjectionAsync(serviceProvider, eventEntity, cancellationToken);
                await this.PublishToMessageBus(serviceProvider, eventEntity, cancellationToken);
                await outboxMessageStatusUpdater.UpdateMessageStatus(outboxMessage.Id, ProcessingStatus.Processed, cancellationToken);

                await mongoUnitOfWork.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                await mongoUnitOfWork.RollbackAsync(cancellationToken);
                await outboxMessageStatusUpdater.UpdateMessageStatus(outboxMessage.Id, ProcessingStatus.Failed, cancellationToken);
            }
        }
        private async Task<EventEntity> GetEventEntityAsync(IServiceProvider services, MessageEntity outboxMessage, CancellationToken cancellationToken)
        {
            var eventStoreReader = services.GetRequiredService<IEventStoreReader>();

            return await eventStoreReader.GetByIdAsync(outboxMessage.EventId!, cancellationToken)
                ?? throw new ArgumentException($"Event - {outboxMessage.EventId} does not exist."); ;
        }

        private async Task RunProjectionAsync(IServiceProvider services, EventEntity eventEntity, CancellationToken cancellationToken)
        {
            var projectionHandler = services.GetRequiredService<IProjectionHandler>();
            await projectionHandler.HandleAsync(eventEntity, cancellationToken);
        }

        private async Task PublishToMessageBus(IServiceProvider services, EventEntity eventEntity, CancellationToken cancellationToken)
        {
            var eventDeserializer = services.GetRequiredService<IEventDeserializer>();
            DomainEvent domainEvent = eventDeserializer.Deserialize(eventEntity);

            var domainEventPublisher = services.GetRequiredService<IDomainEventPublisher>();
            await domainEventPublisher.PublishAsync(domainEvent, cancellationToken);
        }
    }
}
