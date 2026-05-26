using MongoDB.Driver;

namespace Gym.AuthorizationServer.Entities.Users.FormCredentials
{
    public interface IFormCredentialRepository
    {
        Task<FormCredentialEntity?> GetByIdAsync(String id, CancellationToken cancellationToken);
        Task<FormCredentialEntity?> GetByUsernameAsync(String username, CancellationToken cancellationToken);
        Task AddAsync(FormCredentialEntity entity, CancellationToken cancellationToken);
        Task<Boolean> ExistsAsync(String id, CancellationToken cancellationToken);
    }

    public class FormCredentialRepository(IMongoCollection<FormCredentialEntity> _formCredentials) : IFormCredentialRepository
    {
        public async Task AddAsync(FormCredentialEntity entity, CancellationToken cancellationToken)
        {
            await _formCredentials.InsertOneAsync(entity, cancellationToken: cancellationToken);
        }

        public async Task<Boolean> ExistsAsync(String id, CancellationToken cancellationToken)
        {
            return await _formCredentials.Find(eFormCredential => eFormCredential.Id == id)
                .AnyAsync(cancellationToken);
        }

        public async Task<FormCredentialEntity?> GetByIdAsync(String id, CancellationToken cancellationToken)
        {
            return await _formCredentials.Find(eFormCredential => eFormCredential.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<FormCredentialEntity?> GetByUsernameAsync(String username, CancellationToken cancellationToken)
        {
            return await _formCredentials.Find(eFormCredential => eFormCredential.Username == username)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
