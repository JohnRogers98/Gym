using Gym.Domain.FormAuthContext;
using Gym.Domain.FormAuthContext.ValueObjects;
using Gym.Infrastructure.Entities.Extensions.Mappings;
using MongoDB.Driver;

namespace Gym.Infrastructure.Entities.Repositories.FormAuths
{
    internal class FormAuthRepository(IMongoCollection<FormAuthEntity> _formAuthCollection, MongoUnitOfWork _mongoUnitOfWork) : IFormAuthRepository
    {
        public async Task<Boolean> ExistsAsync(Login login, CancellationToken cancellationToken)
            => await _formAuthCollection.Find(_mongoUnitOfWork.Session, eFormAuth => eFormAuth.Login == login.Value).AnyAsync(cancellationToken);

        public async Task<FormAuth?> GetByLoginAsync(Login login, CancellationToken cancellationToken)
        {
            var foundedEntity = await _formAuthCollection.Find(_mongoUnitOfWork.Session, eFormAuth => eFormAuth.Login == login.Value)
                .FirstOrDefaultAsync(cancellationToken);

            return foundedEntity?.ToDomain();
        }

        public async Task SaveAsync(FormAuth formAuth, CancellationToken cancellationToken)
        {
            FormAuthEntity formAuthEntity = formAuth.ToEntity();

            await _formAuthCollection.ReplaceOneAsync(
                _mongoUnitOfWork.Session,
                eFormAuth => eFormAuth.Login == formAuthEntity.Login,
                formAuthEntity,
                new ReplaceOptions { IsUpsert = true },
                cancellationToken);
        }
    }
}
