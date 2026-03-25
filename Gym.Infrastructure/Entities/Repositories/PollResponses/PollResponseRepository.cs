using Gym.Domain.PollResponseContext;
using Gym.Domain.PollResponseContext.ValueObjects;
using Gym.Infrastructure.Entities.Extensions.Mappings;
using MongoDB.Driver;

namespace Gym.Infrastructure.Entities.Repositories.PollResponses
{
    internal class PollResponseRepository(IMongoCollection<PollResponseEntity> _pollResponseCollection, MongoUnitOfWork _mongoUnitOfWork) : IPollResponseRepository
    {
        public async Task SaveAsync(PollResponse pollResponse, CancellationToken cancellationToken)
        {
            PollResponseEntity pollResponseEntity = pollResponse.ToEntity();

            await _pollResponseCollection.ReplaceOneAsync(
                _mongoUnitOfWork.Session,
                ePollResponse => ePollResponse.Id == pollResponseEntity.Id,
                pollResponseEntity,
                new ReplaceOptions { IsUpsert = true },
                cancellationToken);
        }

        public async Task<PollResponse?> GetByIdAsync(PollResponseId id, CancellationToken cancellationToken)
        {
            var foundedEntity = await _pollResponseCollection.Find(_mongoUnitOfWork.Session, ePollResponse => ePollResponse.Id == id.Value)
                .FirstOrDefaultAsync(cancellationToken);

            return foundedEntity?.ToDomain();
        }

        public async Task<Boolean> ExistsAsync(PollResponseId id, CancellationToken cancellationToken)
            => await _pollResponseCollection.Find(_mongoUnitOfWork.Session, ePollResponse => ePollResponse.Id == id.Value).AnyAsync(cancellationToken);
    }
}
