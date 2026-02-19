namespace Gym.Infrastructure.Entities.Repositories.Accounts.EventsDto
{
    internal record TrainingBookedDto(
        String Id,
        DateTime OccuredOn,
        String BookingId,
        String UserId,
        String CalendarEventId
        );
}
