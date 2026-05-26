using Gym.AuthorizationServer.Entities.UserConsents;

namespace Gym.AuthorizationServer.Services
{
    public interface IConsentEvaluationService
    {
        Task<Boolean> NeedsConsentAsync(List<String> requestedScopes, String clientId, String userId);
    }

    public class ConsentEvaluationService(IUserConsentRepository _userConsentRepository) : IConsentEvaluationService
    {
        public async Task<Boolean> NeedsConsentAsync(List<String> requestedScopes, String clientId, String userId)
        {
            var existingConsent = await _userConsentRepository.GetByUserIdAndClientIdAsync(userId, clientId, CancellationToken.None);

            if (existingConsent is null)
                return true;

            var existingScopesSet = new HashSet<String>(existingConsent.GrantedScopes);
            return requestedScopes.Any(scope => !existingScopesSet.Contains(scope));
        }
    }
}
