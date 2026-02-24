using Gym.Abstractions.MessageBus.Publishers;
using Gym.Abstractions.Query.CalendarEvents;
using Gym.Domain.AccountContext.Events;
using Gym.Domain.UserContext;
using MediatR;

namespace Gym.Abstractions.MessageBus.Notifiers.Bookings
{
    internal class NotifyUserAboutBooking(INotificationService _notificationService, ICalendarEventProjectionQueryService _calendarEventProjectionQueryService) 
        : INotificationHandler<DomainEventNotification<TrainingBookedDomainEvent>>
    {
        public async Task Handle(DomainEventNotification<TrainingBookedDomainEvent> notification, CancellationToken cancellationToken)
        {

            CalendarEventProjection? bookedCalendarEvent = 
                await _calendarEventProjectionQueryService.GetByIdAsync(notification.DomainEvent.CalendarEventId.Value, cancellationToken);


            if (bookedCalendarEvent is not null)
            {
                await _notificationService.SendMessageAsync(
                    notification.DomainEvent.UserId,
                    $"You booked event - {bookedCalendarEvent.Training.Name}.",
                    cancellationToken);
            }
        }
    }
}
