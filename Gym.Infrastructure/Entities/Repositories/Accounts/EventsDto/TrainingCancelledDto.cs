namespace Gym.Infrastructure.Entities.Repositories.Accounts.EventsDto
{
    internal record TrainingCancelledDto(String Id, DateTime OccurredOn, String BookingId, String UserId, String CalendarEventId);
}
