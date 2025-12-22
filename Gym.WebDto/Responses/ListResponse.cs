namespace Gym.WebDto.Responses
{
    public record ListResponse<TResourceItem>(IEnumerable<TResourceItem> data);
}
