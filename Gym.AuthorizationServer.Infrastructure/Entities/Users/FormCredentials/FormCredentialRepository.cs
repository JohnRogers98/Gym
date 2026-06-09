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
    }
}
