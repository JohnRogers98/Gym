using Gym.Abstractions.Query.CalendarEvents;
using Gym.Domain.CalendarEventContext;
using Gym.Domain.CalendarEventContext.Events;
using Gym.Infrastructure.Entities.EventStores;
using Gym.Infrastructure.Entities.EventStores.DtoDeserializers;
using Gym.Infrastructure.Entities.Extensions;
using Gym.Infrastructure.Entities.Repositories.CalendarEvents.EventsDto;
using Gym.Infrastructure.Entities.Repositories.Instructors;
using Gym.Infrastructure.Entities.Repositories.Trainings;
using MongoConsoleApp.Repositories.CalendarEvents;
using MongoDB.Driver;

namespace Gym.Infrastructure.Entities.Projections.CalendarEvents
{
    internal class CreatedCalendarEventProjectionHandler(
        IMongoCollection<CalendarEventEntity> _calendarEventCollection,
        IMongoCollection<TrainingEntity> _trainingColletion,
        IMongoCollection<InstructorEntity> _instructorColletion,
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
                instructors = await _instructorColletion
                    .Find(instructor => instructoresList.Contains(instructor.Id))
                    .Project(instructor => new InstructorInfo(
                        instructor.Id.ToString(),
                        String.Concat(instructor.FirstName, " ", instructor.LastName)))
                    .ToListAsync(cancellationToken);
            }


            var projection = new CalendarEventProjection(
                Id: calendarEvent.Id.ToString(),
                Start: calendarEvent.Start,
                End: calendarEvent.End,
                Training: training,
                MaxClientCount: calendarEvent.MaxClientCount,
                BookingUsers: calendarEvent.Bookings?
                    .Select(userId => new BookingUserInfo(userId.ToString()))
                    .ToList(),
                Instructors: instructors
            );

            await _projectionCollection.InsertOneAsync(_mongoUnitOfWork.Session, projection, cancellationToken: cancellationToken);
        }
    }
}
