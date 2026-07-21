using Gym.AuthorizationServer.Infrastructure.Session;
using MongoDB.Driver;

namespace Gym.AuthorizationServer.Infrastructure.Entities.Users.FormCredentials
{
    public interface IFormCredentialRepository
    {
        Task<FormCredentialEntity?> GetByIdAsync(String id, CancellationToken cancellationToken);
        Task<FormCredentialEntity?> GetByUsernameAsync(String username, CancellationToken cancellationToken);
        Task AddAsync(FormCredentialEntity entity, CancellationToken cancellationToken);
        Task<Boolean> ExistsAsync(String id, CancellationToken cancellationToken);
        Task<Boolean> ExistsByUsernameAsync(String username, CancellationToken cancellationToken);
        Task<FormCredentialEntity?> GetByUserIdAsync(String userId, CancellationToken cancellationToken);
        Task UpdatePasswordAsync(String id, String newHashedPassword, CancellationToken cancellationToken);
    }

    public class FormCredentialRepository(IMongoCollection<FormCredentialEntity> _formCredentials, MongoUnitOfWork _mongoUnitOfWork) : IFormCredentialRepository
    {
        public async Task AddAsync(FormCredentialEntity entity, CancellationToken cancellationToken)
        {
            await _formCredentials.InsertOneAsync(_mongoUnitOfWork.Session, entity, cancellationToken: cancellationToken);
        }

        public async Task<Boolean> ExistsAsync(String id, CancellationToken cancellationToken)
        {
            return await _formCredentials.Find(_mongoUnitOfWork.Session, eFormCredential => eFormCredential.Id == id)
                .AnyAsync(cancellationToken);
        }

        public async Task<Boolean> ExistsByUsernameAsync(String username, CancellationToken cancellationToken)
        {
            return await _formCredentials.Find(_mongoUnitOfWork.Session, eFormCredential => eFormCredential.Username == username)
                .AnyAsync(cancellationToken);
        }

        public async Task<FormCredentialEntity?> GetByIdAsync(String id, CancellationToken cancellationToken)
        {
            return await _formCredentials.Find(_mongoUnitOfWork.Session, eFormCredential => eFormCredential.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<FormCredentialEntity?> GetByUsernameAsync(String username, CancellationToken cancellationToken)
        {
            return await _formCredentials.Find(_mongoUnitOfWork.Session, eFormCredential => eFormCredential.Username == username)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<FormCredentialEntity?> GetByUserIdAsync(String userId, CancellationToken cancellationToken)
        {
            return await _formCredentials.Find(_mongoUnitOfWork.Session, eFormCredential => eFormCredential.UserId == userId)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task UpdatePasswordAsync(String id, String newHashedPassword, CancellationToken cancellationToken)
        {
            var filter = Builders<FormCredentialEntity>.Filter.Eq(x => x.Id, id);

            var update = Builders<FormCredentialEntity>.Update
                .Set(x => x.HashedPassword, newHashedPassword);

            await _formCredentials.UpdateOneAsync(_mongoUnitOfWork.Session, filter, update, cancellationToken: cancellationToken);
        }
    }
}
