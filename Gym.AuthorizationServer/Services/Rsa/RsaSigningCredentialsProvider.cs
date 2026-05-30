using Microsoft.IdentityModel.Tokens;

namespace Gym.AuthorizationServer.Services.Rsa
{
    public interface IRsaSigningCredentialsProvider
    {
        SigningCredentials GetSigningCredentials();
    }

    public class RsaSigningCredentialsProvider : IRsaSigningCredentialsProvider
    {
        private readonly SigningCredentials _credentials;

        public RsaSigningCredentialsProvider(IRsaSecurityKeyProvider _rsaSecurityKeyProvider, IConfiguration configuration)
        {
            _credentials = new SigningCredentials(_rsaSecurityKeyProvider.GetRsaSecurityKey(), SecurityAlgorithms.RsaSha256);
        }

        public SigningCredentials GetSigningCredentials() => _credentials;
    }
}
