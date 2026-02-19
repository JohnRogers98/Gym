using Gym.Abstractions.Query.CalendarEvents;
using Gym.Domain.CalendarEventContext;
using Gym.Domain.CalendarEventContext.Events;
using Gym.Infrastructure.Entities.EventStores;
using Gym.Infrastructure.Entities.EventStores.Deserializers;
using Gym.Infrastructure.Entities.Extensions;
using Gym.Infrastructure.Entities.Repositories.Instructors;
using Gym.Infrastructure.Entities.Repositories.Trainings;
using MongoConsoleApp.Repositories.CalendarEvents;
using MongoDB.Driver;

namespace Gym.Infrastructure.Entities.Projections.CalendarEvents
{
    internal class CalendarEventProjectionHandler(
        IMongoCollection<CalendarEventEntity> _calendarEventCollection,
        IMongoCollection<TrainingEntity> _trainingColletion,
        IMongoCollection<InstructorEntity> _instructorColletion,
        IMongoCollection<CalendarEventProjection> _projectionCollection,
        MongoUnitOfWork _mongoUnitOfWork,
        IEventDeserializer _eventDeserializer) : IProjectionHandler
    {
        public Boolean CanHandle(String aggregateType, String operation)
        {
            return aggregateType == nameof(CalendarEvent) && operation == nameof(CalendarEventCreatedDomainEvent);
        }

        public async Task HandleAsync(EventEntity eventEntity, CancellationToken cancellationToken)
        {
            var calendarEventCreatedDomainEvent = _eventDeserializer.Deserialize<CalendarEventCreatedDomainEvent>(eventEntity);

            var calendarEvent = await _calendarEventCollection
                .Find(calendarEvent => calendarEvent.Id == calendarEventCreatedDomainEvent.CalendarEventId.Value.ToObjectId())
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

            /* CalendarEventProjection projection = _calendarEventCollection.AsQueryable()
                 .Where(calendarEvent => calendarEvent.Id == calendarEventCreatedDomainEvent.CalendarEventId.Value.ToObjectId())
                 .Select(calendarEvent => 
                     new CalendarEventProjection(
                         Id: calendarEvent.Id.ToString(),
                         Start: calendarEvent.Start,
                         End: calendarEvent.End,

                         Training: _trainingColletion.AsQueryable()
                             .Where(training => training.Id == calendarEvent.TrainingId)
                             .Select(training => new TrainingInfo(
                                 training.Id.ToString(),
                                 training.Name,
                                 training.Description)
                             ).First(),

                         MaxClientCount: calendarEvent.MaxClientCount,

                         BookingUsers: calendarEvent.Bookings != null 
                             ? calendarEvent.Bookings.ToList().Select(userId => new BookingUserInfo(userId.ToString())) 
                             : null,

                         Instructors: calendarEvent.Instructors != null 
                             ? _instructorColletion.AsQueryable()
                                 .Where(instructor => calendarEvent.Instructors.ToList().Contains(instructor.Id))
                                 .Select(instructor => new InstructorInfo(
                                     instructor.Id.ToString(),
                                     $"{instructor.FirstName} {instructor.LastName}")
                                 ).ToList()
                             : null
                     ))
                 .First();*/

            await _projectionCollection.InsertOneAsync(_mongoUnitOfWork.Session, projection, cancellationToken: cancellationToken);
        }
    }
}
