using Gym.AuthorizationServer.Infrastructure.Entities.UserConsents;

namespace Gym.AuthorizationServer.Services
{
    public interface IConsentEvaluationService
    {
        Task<Boolean> NeedsConsentAsync(
            IEnumerable<ScopeInfo> requestedScopes,
            String userId,
            String clientId,
            String protectedResourceId,
            CancellationToken cancellationToken);
    }

    public class ConsentEvaluationService(IUserConsentRepository _userConsentRepository) : IConsentEvaluationService
    {
        public async Task<Boolean> NeedsConsentAsync(
            IEnumerable<ScopeInfo> requestedScopes,
            String userId,
            String clientId,
            String protectedResourceId,
            CancellationToken cancellationToken)
        {
            var existingUserConsent = await _userConsentRepository.GetAsync(userId, clientId, protectedResourceId, cancellationToken);

            if (existingUserConsent is null)
                return true;

            return requestedScopes
                .Except(existingUserConsent.GrantedScopes, ScopeInfoComparer.Instance)
                .Any();
        }
    }
}
