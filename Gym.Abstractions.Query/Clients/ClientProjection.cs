namespace Gym.Abstractions.Query.Clients
{
    public record ClientProjection(
        String Id,
        String? Username,
        String? FirstName,
        String? LastName,
        Int32 AvailableTrainingsCount 
        );
}
