namespace Gym.Application.Services.AccountApi
{
    public record AccountEventDetails(
        Int32 Version,
        String Operation,
        DateTime OccurredAt,
        Dictionary<String, Object> Data);
}
