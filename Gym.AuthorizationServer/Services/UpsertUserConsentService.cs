using Gym.AuthorizationServer.Infrastructure.Entities.UserConsents;

namespace Gym.AuthorizationServer.Services
{
    public interface IUpsertUserConsentService
    {
        Task<UserConsentEntity> UpsertAsync(List<String> scopes, String clientId, String userId, CancellationToken cancellationToken);
    }

    public class UpsertUserConsentService(IUserConsentRepository _userConsentRepository) : IUpsertUserConsentService
    {
        public async Task<UserConsentEntity> UpsertAsync(List<String> requestedScopes, String clientId, String userId, CancellationToken cancellationToken)
        {
            UserConsentEntity? userConsent = await _userConsentRepository.GetByUserIdAndClientIdAsync(userId, clientId, cancellationToken);
            if (userConsent is null)
            {
                userConsent = new()
                {
                    ClientId = clientId,
                    UserId = userId,
                    GrantedScopes = requestedScopes,
                    GrantedAt = DateTime.UtcNow,
                };

                await _userConsentRepository.AddAsync(userConsent, cancellationToken);
            }
            else
            {
                var mergedScopes = userConsent.GrantedScopes
                        .Union(requestedScopes)
                        .ToList();

                await _userConsentRepository.UpdateGrantedScopesAsync(userId, clientId, mergedScopes, DateTime.UtcNow, cancellationToken);
            }

            return userConsent;
        }
    }

}
