using Gym.Abstractions.Query._CommonInfos;
using Gym.Abstractions.Query.CalendarEvents;
using Gym.Domain.CalendarEventContext;
using Gym.Domain.CalendarEventContext.Events;
using Gym.Infrastructure.Entities.EventStores;
using Gym.Infrastructure.Entities.EventStores.DtoDeserializers;
using Gym.Infrastructure.Entities.Extensions;
using Gym.Infrastructure.Entities.Repositories.CalendarEvents;
using Gym.Infrastructure.Entities.Repositories.CalendarEvents.EventsDto;
using Gym.Infrastructure.Entities.Repositories.Polls;
using Gym.Infrastructure.Entities.Repositories.Trainings;
using Gym.Infrastructure.Entities.Repositories.Users;
using MongoDB.Driver;

namespace Gym.Infrastructure.Entities.Projections.CalendarEvents
{
    internal class CreatedCalendarEventProjectionHandler(
        IMongoCollection<CalendarEventEntity> _calendarEventCollection,
        IMongoCollection<TrainingEntity> _trainingColletion,
        IMongoCollection<UserEntity> _userColletion,
        IMongoCollection<PollEntity> _pollCollection,
        IMongoCollection<CalendarEventProjection> _projectionCollection,
        MongoUnitOfWork _mongoUnitOfWork,
        IEventDtoDeserializer _eventDtoDeserializer) : IProjectionHandler
    {
        public Boolean CanHandle(String aggregateType, String operation)
        {
            return aggregateType == nameof(CalendarEvent) && operation == nameof(CalendarEventCreatedDomainEvent);
        }

        public async Task HandleAsync(EventEntity eventEntity, CancellationToken cancellationToken)
        {
            var calendarEventCreatedDto = _eventDtoDeserializer.Deserialize<CalendarEventCreatedDto>(eventEntity);

            var calendarEvent = await _calendarEventCollection
                .Find(calendarEvent => calendarEvent.Id == calendarEventCreatedDto.CalendarEventId.ToObjectId())
                .FirstAsync(cancellationToken);

            TrainingInfo training = await _trainingColletion
                    .Find(training => training.Id == calendarEvent.TrainingId)
                    .Project(training => new TrainingInfo(
                        training.Id.ToString(),
                        training.Name,
                        training.Description))
                    .FirstAsync(cancellationToken);

            List<InstructorInfo>? instructors = null;
            if (calendarEvent.Instructors?.Any() is true)
            {
                var instructoresList = calendarEvent.Instructors.ToList();
                instructors = await _userColletion
                    .Find(user => instructoresList.Contains(user.Id))
                    .Project(user => new InstructorInfo(
                        user.Id.ToString(),
                        String.Concat(user.FirstName, " ", user.LastName)))
                    .ToListAsync(cancellationToken);
            }

            PollInfo? pollInfo = null;
            if(calendarEvent.PollId.HasValue)
            {
                var poll = await _pollCollection
                    .Find(ePoll => ePoll.Id == calendarEvent.PollId.Value)
                    .FirstAsync(cancellationToken);

                pollInfo = new PollInfo(poll.Id.ToString(), poll.Title, poll.IsRequired, poll.CanAcceptManyChoices, [.. poll.Choices.Select(aChoice => new ChoiceInfo(aChoice.Id, aChoice.Text))]);
            }

            var projection = new CalendarEventProjection(
                Id: calendarEvent.Id.ToString(),
                Start: calendarEvent.Start,
                End: calendarEvent.End,
                Status: calendarEvent.Status,
                Training: training,
                MaxClientCount: calendarEvent.MaxClientCount,
                BookingUsers: calendarEvent.Bookings?
                    .Select(userId => new BookingUserInfo(userId.ToString()))
                    .ToList(),
                instructors,
                pollInfo
            );

            await _projectionCollection.InsertOneAsync(_mongoUnitOfWork.Session, projection, cancellationToken: cancellationToken);
        }
    }
}
