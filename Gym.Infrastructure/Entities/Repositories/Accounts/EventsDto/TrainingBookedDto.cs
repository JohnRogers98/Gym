namespace Gym.Infrastructure.Entities.Repositories.Accounts.EventsDto
{
    internal record TrainingBookedDto(
        String Id,
        DateTime occurredOn,
        String BookingId,
        String UserId,
        String CalendarEventId
        );
}
