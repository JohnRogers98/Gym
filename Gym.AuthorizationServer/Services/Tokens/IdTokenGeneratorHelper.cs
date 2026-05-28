using Gym.AuthorizationServer.Claims;

namespace Gym.AuthorizationServer.Services.Tokens
{
    public interface IIdTokenGeneratorHelper
    {
        String GenerateToken(
            String accessToken,
            String userId,
            String clientId,
            String? nonce = null,
            String? acr = null,
            List<String>? amr = null);
    }

    public class IdTokenGeneratorHelper(
        IIdTokenGenerator _idTokenGenerator,
        IComputeOpenIdAtHashService _computeOpenIdAtHashService,
        IHttpContextAccessor _httpContextAccessor) : IIdTokenGeneratorHelper
    {
        public String GenerateToken(
            String accessToken,
            String userId, 
            String clientId, 
            String? nonce = null,
            String? acr = null,
            List<String>? amr = null)
        {
            IdToken idToken = new()
            {
                Issuer = _httpContextAccessor.HttpContext!.GetBaseUrl(),
                Subject = userId,
                Audience = clientId,
                Expiration = DateTimeOffset.UtcNow.AddMinutes(2).ToUnixTimeSeconds(),
                IssuedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Nonce = nonce,
                AuthenticationContextClassReference = acr,
                AuthenticationMethodsReferences = amr,
                AtHash = _computeOpenIdAtHashService.Compute(accessToken)
            };

            return _idTokenGenerator.GenerateToken(idToken);
        }
    }
}
