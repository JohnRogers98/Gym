namespace Gym.Abstractions.Query.EventStore
{
    public interface IEventProjectionQueryService
    {
        Task<IEnumerable<EventProjection>> GetByStreamId(String streamId, CancellationToken cancellationToken);
    }
}
