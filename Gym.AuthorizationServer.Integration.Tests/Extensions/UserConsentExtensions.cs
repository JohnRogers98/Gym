using Gym.AuthorizationServer.Services.Tokens;

namespace Gym.AuthorizationServer.Infrastructure.Entities.UserConsents
{
    internal static class UserConsentExtensions
    {
        public static AccessTokenClaimsMetadata ToAccessTokenClaimsMetadata(this UserConsentEntity userConsent)
        {
            return new AccessTokenClaimsMetadata
            {
                ClientId = userConsent.ClientId,
                UserId = userConsent.UserId,
                GrantedScopes = userConsent.GrantedScopes
            };
        }
    }
}
