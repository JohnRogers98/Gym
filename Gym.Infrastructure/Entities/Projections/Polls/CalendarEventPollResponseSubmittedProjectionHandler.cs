using Gym.Abstractions.Query.CalendarEvents;
using Gym.Domain.PollContext;
using Gym.Domain.PollResponseContext;
using Gym.Domain.PollResponseContext.Events;
using Gym.Infrastructure.Entities.EventStores;
using Gym.Infrastructure.Entities.EventStores.DtoDeserializers;
using Gym.Infrastructure.Entities.Extensions;
using Gym.Infrastructure.Entities.Repositories.CalendarEvents;
using Gym.Infrastructure.Entities.Repositories.PollResponses;
using Gym.Infrastructure.Entities.Repositories.PollResponses.EventsDto;
using Gym.Infrastructure.Entities.Repositories.Polls;
using MongoDB.Driver;

namespace Gym.Infrastructure.Entities.Projections.Polls
{
    internal class CalendarEventPollResponseSubmittedProjectionHandler(
        IMongoCollection<PollEntity> _pollCollection,
        IMongoCollection<PollResponseEntity> _pollResponseCollection,
        IMongoCollection<CalendarEventEntity> _calendarEventCollection,
        IMongoCollection<CalendarEventProjection> _projectionCollection,
        MongoUnitOfWork _mongoUnitOfWork,
        IEventDtoDeserializer _eventDtoDeserializer) : IProjectionHandler
    {
        public Boolean CanHandle(String aggregateType, String operation)
        {
            return aggregateType == nameof(PollResponse) && operation == nameof(CalendarEventPollResponseCreatedDomainEvent);
        }

        public async Task HandleAsync(EventEntity eventEntity, CancellationToken cancellationToken)
        {
            var calendarEventPollResponseSubmittedDto = _eventDtoDeserializer.Deserialize<CalendarEventPollResponseCreatedDto>(eventEntity);

            var pollResponseEntity = await _pollResponseCollection
                .Find(ePollResponse => ePollResponse.Id == calendarEventPollResponseSubmittedDto.PollResponseId)
                .FirstAsync(cancellationToken);

            var pollEntity = await _pollCollection
                .Find(ePoll => ePoll.Id == calendarEventPollResponseSubmittedDto.PollId.ToObjectId())
                .FirstAsync(cancellationToken);

            var calendarEvent = await _calendarEventCollection
                .Find(eCalendarEvent => eCalendarEvent.PollId == calendarEventPollResponseSubmittedDto.PollId.ToObjectId())
                .FirstAsync(cancellationToken);

            var projectionEntity = await _projectionCollection
                .Find(projection => projection.Id == calendarEvent.Id.ToString())
                .FirstAsync(cancellationToken);

            var choiceIds = pollResponseEntity.ChoiceIds.ToList();

            var updatedChoices = projectionEntity.PollInfo!.Choices
                .Select(choice => choiceIds.Contains(choice.Id) ? choice with { VoteCount = choice.VoteCount + 1 }: choice)
                .ToList();

            var updatedProjection = projectionEntity with
            {
                PollInfo = projectionEntity.PollInfo with
                {
                    Choices = updatedChoices
                }
            };

            await _projectionCollection.ReplaceOneAsync(
                _mongoUnitOfWork.Session,
                eProjection => eProjection.Id == projectionEntity.Id,
                updatedProjection,
                cancellationToken: cancellationToken
            );
        }
    }
}
