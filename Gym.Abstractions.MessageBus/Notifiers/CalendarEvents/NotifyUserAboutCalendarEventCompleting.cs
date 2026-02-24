using Gym.Abstractions.MessageBus.Publishers;
using Gym.Abstractions.Query.CalendarEvents;
using Gym.Domain._Shared;
using Gym.Domain.CalendarEventContext.Events;
using Gym.Domain.UserContext;
using MediatR;

namespace Gym.MessageBus.Notifiers.CalendarEvents
{
    internal class NotifyUserAboutCalendarEventCompleting(INotificationService _notificationService, ICalendarEventProjectionQueryService _calendarEventProjectionQueryService)
         : INotificationHandler<DomainEventNotification<CalendarEventCompletedDomainEvent>>
    {
        public async Task Handle(DomainEventNotification<CalendarEventCompletedDomainEvent> notification, CancellationToken cancellationToken)
        {

            CalendarEventProjection? bookedCalendarEvent =
                await _calendarEventProjectionQueryService.GetByIdAsync(notification.DomainEvent.CalendarEventId.Value, cancellationToken);


            if (bookedCalendarEvent is null)
                return;

            foreach (UserId aBookingUser in notification.DomainEvent.BookingUsers)
            {
                await _notificationService.SendMessageAsync(
                    aBookingUser,
                    $"Event - {bookedCalendarEvent.Training.Name} completed.",
                    cancellationToken);
            }
        }
    }
}
