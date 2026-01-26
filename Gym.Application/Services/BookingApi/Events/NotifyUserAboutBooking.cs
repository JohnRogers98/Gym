using Gym.Application.Services.DomainEventPublisher;
using Gym.Domain.BookingAggregate.Events;
using Gym.Domain.CalendarEventAggregate;
using Gym.Domain.UserAggregate;
using MediatR;

namespace Gym.Application.Services.BookingApi.Events
{
    internal class NotifyUserAboutBooking(INotificationService _notificationService, ICalendarEventQueryService _calendarEventQueryService) 
        : INotificationHandler<DomainEventNotification<TrainingBookedDomainEvent>>
    {
        public async Task Handle(DomainEventNotification<TrainingBookedDomainEvent> notification, CancellationToken cancellationToken)
        {

            CalendarEvent? bookedCalendarEvent = await _calendarEventQueryService.GetByIdAsync(notification.DomainEvent.CalendarEventId, cancellationToken);

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
