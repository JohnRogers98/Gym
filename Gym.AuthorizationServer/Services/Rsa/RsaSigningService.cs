using Microsoft.IdentityModel.Tokens;

namespace Gym.AuthorizationServer.Services.Rsa
{
    public interface IRsaSigningService
    {
        SigningCredentials GetSigningCredentials();
    }

    public class RsaSigningService : IRsaSigningService
    {
        private readonly SigningCredentials _credentials;

        public RsaSigningService(IRsaKeyService _rsaKeyService)
        {
            var securityKey = new RsaSecurityKey(_rsaKeyService.GetRsa());
            _credentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256);
        }

        public SigningCredentials GetSigningCredentials() => _credentials;
    }
}
