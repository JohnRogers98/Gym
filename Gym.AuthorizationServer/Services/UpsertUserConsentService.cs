using Gym.AuthorizationServer.Infrastructure.Entities.UserConsents;

namespace Gym.AuthorizationServer.Services
{
    public interface IUpsertUserConsentService
    {
        Task<UserConsentEntity> UpsertAsync(
            IEnumerable<ScopeInfo> requestedGrantedScopes,
            String userId,
            String clientId,
            String protectedResourceId,
            CancellationToken cancellationToken);
    }

    public class UpsertUserConsentService(IUserConsentRepository _userConsentRepository) : IUpsertUserConsentService
    {
        public async Task<UserConsentEntity> UpsertAsync(
            IEnumerable<ScopeInfo> requestedGrantedScopes,
            String userId,
            String clientId,
            String protectedResourceId,
            CancellationToken cancellationToken)
        {
            UserConsentEntity? userConsent = await _userConsentRepository.GetAsync(userId, clientId, protectedResourceId, cancellationToken);
            if (userConsent is null)
            {
                userConsent = new()
                {
                    ClientId = clientId,
                    UserId = userId,
                    ProtectedResourceId = protectedResourceId,
                    GrantedScopes = requestedGrantedScopes.ToList(),
                    GrantedAt = DateTime.UtcNow,
                };

                await _userConsentRepository.AddAsync(userConsent, cancellationToken);
            }
            else
            {
                var mergedScopes = this.MergeScopes(userConsent.GrantedScopes, requestedGrantedScopes);
                await _userConsentRepository.UpdateGrantedScopesAsync(userId, clientId, mergedScopes, DateTime.UtcNow, cancellationToken);
            }

            return userConsent;
        }

        private List<ScopeInfo> MergeScopes(IEnumerable<ScopeInfo> sourceScope, IEnumerable<ScopeInfo> targetScope)
        {
            return sourceScope.Union(targetScope, ScopeInfoComparer.Instance).ToList();
        }
    }

}
