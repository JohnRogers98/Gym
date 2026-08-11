using Gym.AuthorizationServer.Services.Rsa;
using System.Security.Cryptography;

namespace Gym.AuthorizationServer.Integration.Tests.Fakes
{
    internal class FakeRsaKeyProvider : IRsaKeyProvider
    {
        private RSA? _rsa;

        public RSA GetRsa()
        {
            if (_rsa is not null)
                return _rsa;

            _rsa = RSA.Create(2048);
            return _rsa;
        }
    }
}
