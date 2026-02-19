namespace Gym.WebDto.Responses.Account
{
    public record AccountHistoryDto
    {
        public required Int32 Version { get; init; }

        public required String Operation { get; init; }

        public required Dictionary<String, Object> Data { get; init; }

        public required DateTime OccurredAt { get; init; }
    }
}
