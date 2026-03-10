namespace Gym.Infrastructure.Entities.Repositories.Accounts.EventsDto
{
    internal record TrainingCompletedDto(String Id, DateTime OccurredOn, String BookingId, String UserId, String CalendarEventId);
}
