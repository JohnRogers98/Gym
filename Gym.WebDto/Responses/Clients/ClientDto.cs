namespace Gym.WebDto.Responses.Clients
{
    public record ClientDto
    {
        public required String Id {  get; init; }
        public String? Username {  get; init; }
        public String? FirstName {  get; init; }
        public String? LastName {  get; init; }
        public required Int32 AvailableTrainingsCount {  get; init; }
    }
}
