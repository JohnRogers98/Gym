namespace Gym.Infrastructure.Entities.Repositories.Accounts.EventsDto
{
    internal record AccountChargedDto(
        String Id,
        DateTime OccuredOn,
        String UserId,
        Int32 ByCount,
        String? Reason
        );
}
