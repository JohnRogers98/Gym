using Gym.Application.Extensions;
using Gym.Domain.PollContext;
using Gym.Domain.PollContext.ValueObjects;
using Gym.Infrastructure.Entities.Extensions;
using Gym.Infrastructure.Entities.Extensions.Mappings;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gym.Infrastructure.Entities.Repositories.Polls
{
    internal class PollRepository(IMongoCollection<PollEntity> _pollCollection, MongoUnitOfWork _mongoUnitOfWork) : IPollRepository
    {
        public PollId NextIdentity() => PollId.From(ObjectId.GenerateNewId().ToString()).Unwrap();

        public async Task SaveAsync(Poll poll, CancellationToken cancellationToken)
        {
            PollEntity pollEntity = poll.ToEntity();

            await _pollCollection.ReplaceOneAsync(
                _mongoUnitOfWork.Session,
                ePoll => ePoll.Id == pollEntity.Id,
                pollEntity,
                new ReplaceOptions { IsUpsert = true },
                cancellationToken);
        }

        public async Task<Poll?> GetByIdAsync(PollId id, CancellationToken cancellationToken)
        {
            var foundedEntity = await _pollCollection.Find(_mongoUnitOfWork.Session, ePoll => ePoll.Id == id.Value.ToObjectId())
               .FirstOrDefaultAsync(cancellationToken);

            return foundedEntity?.ToDomain();
        }

        public async Task<Boolean> ExistsAsync(PollId id, CancellationToken cancellationToken)
            => await _pollCollection.Find(_mongoUnitOfWork.Session, ePoll => ePoll.Id == id.Value.ToObjectId()).AnyAsync(cancellationToken);
    }
}
